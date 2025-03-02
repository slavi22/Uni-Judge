using System.Reflection;
using System.Text;
using BE.Business.Services.Implementations;
using BE.Business.Services.Interfaces;
using BE.DataAccess.Data;
using BE.DataAccess.Repositories.Implementations;
using BE.DataAccess.Repositories.Interfaces;
using BE.Models.Models.Auth;
using BE.Presentation.ExceptionHandlers;
using BE.Presentation.Policies.Handlers;
using BE.Presentation.Policies.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

// The name could be either "Presentation" or "API" for the main project
namespace BE.Presentation;

// Folder structure for N-Tier Architecture => https://medium.com/@csns.giri/guide-to-building-an-n-tier-architecture-for-a-net-8-web-api-a49f4a83335e
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<AppDbContext>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Secret").Value)),
                    ClockSkew = TimeSpan.Zero //default skew is 5 mins => token is still valid 5 mins after expiring
                };
                options.Events = new JwtBearerEvents
                {
                    //https://code-maze.com/how-to-use-httponly-cookie-in-net-core-for-authentication-and-refresh-token-actions/
                    //https://www.youtube.com/watch?v=Qm64zinOVpc
                    OnMessageReceived = context =>
                    {
                        context.Request.Cookies.TryGetValue("accessToken", out var accessToken);
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(new ProblemDetails
                        {
                            Status = StatusCodes.Status403Forbidden,
                            Title = "Access Denied!",
                            Detail = "You don't have the correct privileges to access this page."
                        });
                    },
                    //https://stackoverflow.com/a/70885152
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsJsonAsync(new ProblemDetails
                        {
                            Status = StatusCodes.Status401Unauthorized,
                            Title = "Access Denied!",
                            Detail = "You are not logged in to access this page."
                        });
                    }
                };
            })
            .AddGoogleOpenIdConnect(options =>
            {
                options.ClientId = builder.Configuration.GetSection("Authentication:Google:ClientId").Value;
                options.ClientSecret = builder.Configuration.GetSection("Authentication:Google:ClientSecret").Value;
                options.SkipUnrecognizedRequests = true;
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("HasSignedUpForCourse", policy =>
            {
                // search google for docs of this "RequireAuthenticatedUser" method
                // https://github.com/dotnet/aspnetcore/issues/4656
                //policy.RequireAuthenticatedUser(); //THIS IS BUGGED AND DOESN'T WORK AT ALL => https://github.com/dotnet/aspnetcore/issues/4656#issuecomment-605012014
                // i need to check if the user is authenticated in the requirement handler for the auth check to work
                policy.Requirements.Add(new StudentHasSignedUpForCourseRequirement());
            });
        });

        //Authorization handlers
        builder.Services.AddScoped<IAuthorizationHandler, StudentHasSignedUpForCourseHandler>();

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            //https://github.com/dotnet/efcore/issues/35110#issuecomment-2517298432
            options.ConfigureWarnings(w =>
                w.Ignore(RelationalEventId
                    .PendingModelChangesWarning)); //used to remove warning about pending migrations
            options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnectionString"));
        });

        // https://www.milanjovanovic.tech/blog/the-right-way-to-use-httpclient-in-dotnet#:~:text=Reducing%20Code%20Duplication%20With%20Named%20Clients
        builder.Services.AddHttpClient("Judge",
            client => { client.BaseAddress = new Uri(builder.Configuration.GetSection("JudgeServerAddress").Value); });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "CorsPolicy", policy =>
            {
                var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins:FEAddress").Value;
                // possibly add AllowCredentials() for the httponly cookies if they don't work
                policy.WithOrigins(allowedOrigins).AllowCredentials().AllowAnyMethod().AllowAnyHeader();
            });
        });

        // Add services to the container.
        builder.Services.AddControllers(options =>
        {
            //https://stackoverflow.com/a/75483959
            //options.Filters.Add(new AuthorizeFilter()); //this will make all controller endpoints require authentication
        }).AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });

        builder.Services.AddExceptionHandler<IncorrectTeacherSecretExceptionHandler>();
        builder.Services.AddExceptionHandler<ProblemNotFoundExceptionHandler>();
        builder.Services.AddExceptionHandler<CourseNotFoundExceptionHandler>();
        builder.Services.AddExceptionHandler<InvalidCoursePasswordExceptionHandler>();
        builder.Services.AddExceptionHandler<DuplicateCourseIdExceptionHandler>();
        // Global exception handler should be added last to catch all other unhandled exceptions
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        builder.Services.AddProblemDetails();

        //Repositories
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IMainMethodBodiesRepository, MainMethodBodiesRepository>();
        builder.Services.AddScoped<IProblemRepository, ProblemRepository>();
        builder.Services.AddScoped<IUserSubmissionRepository, UserSubmissionRepository>();
        builder.Services.AddScoped<ICourseRepository, CourseRepository>();

        // Services
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IJudgeService, JudgeService>();
        builder.Services.AddScoped<IProblemService, ProblemService>();
        builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
        builder.Services.AddScoped<IUserSubmissionService, UserSubmissionService>();
        builder.Services.AddScoped<ICourseService, CourseService>();
        builder.Services.AddScoped<IAuthProvidersService, AuthProvidersService>();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        // builder.Services.AddOpenApi();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGenNewtonsoftSupport();
        builder.Services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "Uni-Judge Backend", Version = "v1",
                    Description = "Uni-Judge Backend API used for code assignments evaluation."
                });
            //https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-8.0&tabs=visual-studio#:~:text=var%20xmlFilename%20%3D%20%24%22%7BAssembly.GetExecutingAssembly().GetName().Name%7D.xml%22%3B%0A%20%20%20%20options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory%2C%20xmlFilename))%3B
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            setup.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            var securitySchema = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter you JWT Bearer token in this field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = JwtBearerDefaults.AuthenticationScheme
            };
            setup.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);
            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    new string[] { }
                }
            };
            setup.AddSecurityRequirement(securityRequirement);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        app.UseExceptionHandler();

        app.UseHttpsRedirection();

        app.UseCors("CorsPolicy");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}