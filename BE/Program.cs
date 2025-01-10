using System.Reflection;
using System.Text;
using BE.Data;
using BE.ExceptionHandlers;
using BE.Models.Auth;
using BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Services.Implementations;
using BE.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace BE;

public class Program
{
    //TODO: DOCUMENT EVERYTHING BEFORE CONTINUING
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
        }).AddJwtBearer(options =>
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
        });

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            //https://github.com/dotnet/efcore/issues/35110#issuecomment-2517298432
            options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)); //used to remove warning about pending migrations
            options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnectionString"));
        });

        builder.Services.AddHttpClient("Judge", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration.GetSection("JudgeServerAddress").Value);
        });

        // Add services to the container.
        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });

        builder.Services.AddExceptionHandler<IncorrectTeacherSecretExceptionHandler>();
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


        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}