certs: Server/blueberry.Server.pfx

Server/blueberry.Server.pfx:
	@dotnet dev-certs https && ln $(shell ls ~/.dotnet/corefx/cryptography/x509stores/my/*.pfx) $(shell pwd)/Server/blueberry.Server.pfx

clean-certs:
	@unlink Server/blueberry.Server.pfx && dotnet dev-certs https --clean