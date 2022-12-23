# syntax=docker/dockerfile:1

####################################
# Build runtime image
####################################
FROM mcr.microsoft.com/dotnet/runtime:6.0.1-focal
ARG CONFIGURATION=Release
ARG VERSION

WORKDIR /app

RUN echo Configuration = ${CONFIGURATION}
RUN echo Version = ${VERSION}

EXPOSE 80 11111 30000

COPY ./Source/out/x64/*.dll .
COPY ./Source/out/x64/*.json .
COPY ./Source/out/x64/*.so .
COPY ./Source/out/x64/IngressMiddleware .
COPY ./Source/wwwroot ./wwwroot

ENTRYPOINT ["./IngressMiddleware"]
