prepare-local: seed-db certs connection-string

clean: clean-certs db-rm clean-secrets

clean-certs:
	unlink Server/blueberry.Server.pfx || true
	dotnet dev-certs https --clean

clean-secrets:
	dotnet user-secrets clear --project Server

certs: Server/blueberry.Server.pfx

Server/blueberry.Server.pfx:
	docker build -t blueberry-gen-certs certs
	docker run --rm -v "$(shell realpath ./certs):/out" blueberry-gen-certs blueberryproject.onmicrosoft.com
	cp $(shell ls ./certs/*.pfx) ./Server/blueberry.Server.pfx

db-up:
	docker start blueberry-db 2>/dev/null || docker run --name blueberry-db -d -e 'SA_PASSWORD=TESTTESTTEST123:)' -e 'ACCEPT_EULA=Y' -p "1433:1433" mcr.microsoft.com/mssql/server:2019-latest && sleep 10

db-stop:
	docker stop blueberry-db

db-rm: db-stop
	docker rm blueberry-db

seed-db: db-up
	ConnectionString='Server=localhost;Database=blueberry;User Id=sa;Password=TESTTESTTEST123:)' dotnet run --project Server -- seed

connection-string:
	dotnet user-secrets init --project Server && dotnet user-secrets set 'ConnectionStrings:Blueberry' 'Server=localhost;Database=blueberry;User Id=sa;Password=TESTTESTTEST123:)' --project Server

.PHONY: clean-certs certs db-up db-stop db-rm seed-db connection-string prepare-local