FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
# http
EXPOSE 5127
# https
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BE.Presentation/BE.Presentation.csproj", "BE.Presentation/"]
COPY ["BE.Models/BE.Models.csproj", "BE.Models/"]
COPY ["BE.DTOs/BE.DTOs.csproj", "BE.DTOs/"]
COPY ["BE.Common/BE.Common.csproj", "BE.Common/"]
COPY ["BE.Business/BE.Business.csproj", "BE.Business/"]
COPY ["BE.DataAccess/BE.DataAccess.csproj", "BE.DataAccess/"]
COPY ["BE.Extensions/BE.Extensions.csproj", "BE.Extensions/"]
RUN dotnet restore "BE.Presentation/BE.Presentation.csproj"
COPY . .
WORKDIR "/src/BE.Presentation"
RUN dotnet build "BE.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BE.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
# Apply last migration - didn't work, could be useful for the future on how to install tools inside the docker container
#RUN dotnet tool install --global dotnet-ef --version 9.0.3
#ENV PATH="$PATH:/root/.dotnet/tools"
#RUN dotnet ef database update

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BE.Presentation.dll"]
