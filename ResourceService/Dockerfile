FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 7000

ENV ASPNETCORE_URLS=http://+:7000;http://+:7001
ENV ASPNETCORE_ENVIRONMENT ASPNETCORE_ENVIRONMENT
ENV RESOURCEDB_CONNECTIONSTRING RESOURCEDB_CONNECTIONSTRING
ENV RUN_MIGRATIONS RUN_MIGRATIONS

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src

# copy all the layers' csproj files into respective folders
COPY ["./src/Core/ResourceService.Domain/ResourceService.Domain.csproj", "src/Core/ResourceService.Domain/"]
COPY ["./src/Core/ResourceService.Application/ResourceService.Application.csproj", "src/Core/ResourceService.Application/"]
COPY ["./src/Infrastructure/ResourceService.Infrastructure/ResourceService.Infrastructure.csproj", "src/Infrastructure/ResourceService.Infrastructure/"]
COPY ["./src/Infrastructure/ResourceService.Persistence/ResourceService.Persistence.csproj", "src/Infrastructure/ResourceService.Persistence/"]
COPY ["./src/Api/ResourceService.Api/ResourceService.Api.csproj", "src/Api/ResourceService.Api/"]

RUN dotnet restore "src/Api/ResourceService.Api/ResourceService.Api.csproj"
COPY . .
WORKDIR /src
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["src/Infrastructure/ResourceService.Infrastructure/Certificates/public.pem", "public.pem"]
COPY ["src/Infrastructure/ResourceService.Infrastructure/Certificates/ImageAccessToken.pem", "ImageAccessToken.pem"]
ENTRYPOINT ["dotnet", "ResourceService.Api.dll"]
