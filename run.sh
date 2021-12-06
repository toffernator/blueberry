docker pull mcr.microsoft.com/mssql/server:2019-latest
export dbpassword="TESTTESTTEST123:)"
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$dbpassword" -p 1433:1433 --name blueberrydb -h blueberrydb -d mcr.microsoft.com/mssql/server:2019-latest
export databasename="blueberry"
export ConnectionString="Server=localhost;Database=$databasename;User Id=sa;Password=$dbpassword;Trusted_Connection=False;Encrypt=True"

dotnet run --project Server -- seed
dotnet run --project Server
