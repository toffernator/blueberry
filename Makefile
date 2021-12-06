certs: Server/blueberry.Server.pfx

Server/blueberry.Server.pfx:
	@dotnet dev-certs https -ep $(shell pwd)/Server/blueberry.Server.pfx

clean-certs:
	@rm $(shell pwd)/Server/blueberry.Server.pfx