# Blueberry Baby

## How to run

Assumes a POSIX terminal. Thus everything must be run in WSL if on Windows.

### Requirements

POSIX terminal
make
docker
dotnet

### Locally

To run the database and server locally, you should run the following commands in the order they are given.

```shell
make
dotnet run --project Server
```

To remove generated artifacts, run

```shell
make clean
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
