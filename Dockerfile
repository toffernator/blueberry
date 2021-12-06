# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY blueberry.sln ./
COPY Common/blueberry.Common.csproj ./Common/
COPY Common.Tests/blueberry.Common.Tests.csproj ./Common.Tests/
COPY Client/blueberry.Client.csproj ./Client/
COPY Core/blueberry.Core.csproj   ./Core/
COPY Infrastructure/blueberry.Infrastructure.csproj   ./Infrastructure/
COPY Infrastructure.Tests/blueberry.Infrastructure.Tests.csproj   ./Infrastructure.Tests/
COPY Server/blueberry.Server.csproj   ./Server/
COPY Server.Tests/blueberry.Server.Tests.csproj ./Server.Tests/
RUN dotnet restore

# Generate dev certs for service
RUN dotnet dev-certs https -ep /cert.pfx

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /cert.pfx ./blueberry.Server.pfx
ENTRYPOINT ["dotnet", "blueberry.Server.dll"]
