# syntax=docker/dockerfile:1

####################################
# Build runtime image
####################################
ARG VERSION
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal

RUN echo Version = ${VERSION}

# Install Tools
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    nano \
    iputils-ping \
    netcat \
    wget \
    curl \
    gnupg \
    apt-transport-https \
    ca-certificates \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Create entrypoint that runs both MongoDB and Runtime
COPY Docker/copy-server-files.sh ./copy-server-files.sh
RUN chmod +x ./copy-server-files.sh

COPY ./Source/out ./out
COPY ./Source/wwwroot ./wwwroot

RUN echo $PWD
RUN ./copy-server-files.sh

EXPOSE 80 11111 30000
ENTRYPOINT ["dotnet", "./IngressMiddleware.dll"]
