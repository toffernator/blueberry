# Blueberry Baby

## How to run

Assumes a POSIX terminal. Thus everything must be run in WSL if on Windows.

**MacOS**: If you are using MacOS you need to install utils (the important being realpath) this can be done runnig:

```bash
$ brew install coreutils
```

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

In some cases the seeding of the database does not work correctly with docker-compose.
If there are fewer than 20,000 database entries, then you need to force a rebuild of
the database container by starting the program with the command below instead:
```shell
docker-compose up --force-recreate --build
```

On MacOS / Windows

```shell
docker compose build && docker compose up
```

### Emergency user

In case authentication with your own Microsoft account fails, we have created a Guest user which will allow you to log in
nonetheless.

Username: guest@blueberryproject.onmicrosoft.com
Password: L2q0!8grGYD2JRELEj2A
