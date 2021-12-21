# Blueberry

## Known Bugs
- There is a single known issue in the searchComponent. When searching on the /material page the tags in the ui are out of sync with the results. I.e. if one searches for the tag 'docker', the correct results are shown but the tag docker is disabled. For more info see #119.
- Given the nature of the data-gen script there is nonsensical data in the database. I.e. 'Working with HTML in PostgreSQL'. 
This is done to make the searching experience interesting and interactive by providing multiple tags to materials. 
- The program is not threadsafe, however since mostly read-actions are performed we have not prioritized this issue. As seen in the load testing section of the README, Blueberry is able to handle multiple concurrent GET-requests, this has been our top priority.

## How to run

Assumes a POSIX terminal. Thus everything must be run in WSL if on Windows.

**MacOS**: If you are using MacOS you need to install utils (the important being realpath) this can be done running:

```bash
$ brew install coreutils
```

### Requirements

POSIX terminal
make
docker
dotnet

### Development

To run the database and server locally, you should run the following commands in the order they are given.

```shell
make
dotnet run --project Server
```

To remove generated artifacts, run

```shell
make clean
```

### Production (With Docker-compose)

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

## Load testing

For load testing see [misc](/Misc)
