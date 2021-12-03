# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY blueberry.sln ./
COPY Client/blueberry.Client.csproj ./Client/
COPY Core/blueberry.Core.csproj   ./Core/
COPY Infrastructure/blueberry.Infrastructure.csproj   ./Infrastructure/
COPY Infrastructure.Tests/blueberry.Infrastructure.Tests.csproj   ./Infrastructure.Tests/
COPY Server/blueberry.Server.csproj   ./Server/
COPY Server.Tests/blueberry.Server.Tests.csproj ./Server.Tests/
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "blueberry.Server.dll"]