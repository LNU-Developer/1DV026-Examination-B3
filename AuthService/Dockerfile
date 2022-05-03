FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 6000
EXPOSE 6001

ENV ASPNETCORE_URLS=http://+:6000;http://+:6001
ENV ASPNETCORE_ENVIRONMENT ASPNETCORE_ENVIRONMENT
ENV AUTHDB_CONNECTIONSTRING AUTHDB_CONNECTIONSTRING
ENV RUN_MIGRATIONS RUN_MIGRATIONS

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
# WORKDIR /src

# copy all the layers' csproj files into respective folders
# COPY ["./src/Core/AuthService.Domain/AuthService.Domain.csproj", "src/Core/AuthService.Domain/"]
COPY ["./src/Core/AuthService.Application/AuthService.Application.csproj", "src/Core/AuthService.Application/"]
# COPY ["./src/Infrastructure/AuthService.Infrastructure/AuthService.Infrastructure.csproj", "src/Infrastructure/AuthService.Infrastructure/"]
# COPY ["./src/Infrastructure/AuthService.Persistence/AuthService.Persistence.csproj", "src/Infrastructure/AuthService.Persistence/"]
COPY ["./src/Infrastructure/AuthService.Identity/AuthService.Identity.csproj", "src/Infrastructure/AuthService.Identity/"]
COPY ["./src/Api/AuthService.Api/AuthService.Api.csproj", "src/Api/AuthService.Api/"]
COPY ["AuthService.sln", "AuthService.sln"]

RUN dotnet restore "src/Api/AuthService.Api/AuthService.Api.csproj"
COPY . .
# WORKDIR /src
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["src/Infrastructure/AuthService.Identity/Certificates/private.pem", "private.pem"]
ENTRYPOINT ["dotnet", "AuthService.Api.dll"]
