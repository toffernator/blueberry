# Blueberry Baby

## How to run

### Locally

To run the database and server locally, you should run the following commands in the order they are given. Since we are
using Make for `make certs` you must either install this dependency or run it using WSL:

```shell
make certs
docker run -e 'SA_PASSWORD=TESTTESTTEST123:)' -e 'ACCEPT_EULA=Y' -p "1433:1433" mcr.microsoft.com/mssql/server:2019-latest
ConnectionString='Server=localhost;Database=blueberry;User Id=sa;Password=TESTTESTTEST123:)' dotnet run --project Server -- seed
ConnectionString='Server=localhost;Database=blueberry;User Id=sa;Password=TESTTESTTEST123:)' dotnet run --project Server
```

### With Docker-compose

To run the database and server locally using docker compose run the following commands:

On linux

```shell
docker-compose build && docker-compose up
```

On MacOS / Windows

```shell
docker compose build && docker compose up
```
