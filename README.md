# Aksio NginxMiddleware

[![C# Build](https://github.com/aksio-insurtech/nginxmiddleware/actions/workflows/dotnet-build.yml/badge.svg)](https://github.com/aksio-insurtech/nginxmiddleware/actions/workflows/dotnet-build.yml)
[![Publish](https://github.com/aksio-insurtech/nginxmiddleware/actions/workflows/publish.yml/badge.svg)](https://github.com/aksio-insurtech/nginxmiddleware/actions/workflows/publish.yml)
[![Docker](https://img.shields.io/docker/v/aksioinsurtech/nginxmiddleware?label=NginxMiddleware&logo=docker&sort=semver)](https://hub.docker.com/r/aksioinsurtech/nginxmiddleware)

This repository holds a middleware used in the hosted environment @ Aksio.
Its job is to look at requests and perform necessary operations, such as inserting the correct `Tenant-ID` header as required by [Cratis](https://github.com/aksio-insurtech/Cratis).

## Running locally

The middleware can be run by navigating to the `./Source` folder and do:

```shell
dotnet run
```

For testing the middleware in a reverse proxy situation, you can run the Docker Compose in `./Sample`:

```shell
docker compose up
```

This will give you a reverse proxy set up on port 8080. Navigate your browser to [http://localhost:8080](http://localhost:8080).

## Cratis Tenant Config

The tenants configuration is done through a well known file called `tenants.json` sitting in the `config`folder next to the binaries of the middleware.
To override it in a running environment all you need to do is mount a volume that overrides either the `config` folder or the specific `config/tenants.json` file.

Its format is:

```json
{
    "<tenant guid>": [
        "<host string>"
    ]
}
```

Each tenant can have multiple host strings it can map to. This is handy for different environments where you prefix the environment with a string (e.g. "dev", "prod", "test", "staging").

To test the `Tenant-ID` injection in the response you can simply add your configured hosts to the operating systems hosts file to point to localhost/127.0.0.1 and then
navigate to the URL + port 8080 with the sample reverse proxy setup.
