# Security

## Authentication


```powershell
# Server
$serverApiClientId = "6965b7b3-fe48-4cc0-899d-7ac7d912acea"
$tenantId = "bf75dc32-a2b0-4c9d-acb1-a086c45e806c"
$tenantDomain = "blueberryproject.onmicrosoft.com"
$appIdUri = "api://6965b7b3-fe48-4cc0-899d-7ac7d912acea/API.Access"
$scope = "API.Access"

## Client
$redirectUri = "https://localhost:7207/authentication/login-callback"
$clientAppClientId = "378387ad-3827-45dc-bf22-5b2c02bd93bb"

dotnet new blazorwasm --hosted `
    --auth SingleOrg `
    --api-client-id "$serverApiClientId" `
    --app-id-uri "$serverApiClientId" `
    --client-id "$clientAppClientId" `
    --default-scope "$scope" `
    --domain "$tenantDomain" `
    --output "MyApp" `
    --tenant-id "$tenantId"
```

```bash
# Server
serverApiClientId="6965b7b3-fe48-4cc0-899d-7ac7d912acea"
tenantId="bf75dc32-a2b0-4c9d-acb1-a086c45e806c"
tenantDomain="blueberryproject.onmicrosoft.com"
appIdUri="api://6965b7b3-fe48-4cc0-899d-7ac7d912acea/API.Access"
scope="API.Access"

## Client
redirectUri="https://localhost:7207/authentication/login-callback"
clientAppClientId="378387ad-3827-45dc-bf22-5b2c02bd93bb"

dotnet new blazorwasm --hosted --auth SingleOrg --api-client-id "$serverApiClientId" --app-id-uri "$serverApiClientId" --client-id "$clientAppClientId" --default-scope "$scope" --domain "$tenantDomain" --tenant-id "$tenantId"
```


## Certificates

Source: <https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-6.0>

```powershell
dotnet dev-certs https --clean
dotnet dev-certs https --export-path $env:USERPROFILE\.aspnet\https\aspnetapp.pfx --password localhost --trust
dotnet dev-certs https --trust
```

## Client Update

```xml
<ItemGroup>
    <TrimmerRootAssembly Include="Microsoft.Authentication.WebAssembly.Msal" />
</ItemGroup>
```

## User Specific Access (Authorization)

## Allow

## Demo next time

Create page for user claims demo
