FROM debian:stretch

LABEL maintainer "Georgy Komarov <jubnzv@gmail.com>"

# Install required packages
ARG DEBIAN_FRONTEND=noninteractive
RUN apt-get update && \
    apt-get -y upgrade && \
    apt-get install -y \
    wget \
    mono-devel \
    mono-xbuild && \
    apt-get autoremove -y; apt-get clean; \
    rm /var/lib/apt/lists/* -r; rm -rf /usr/share/man/*

# Initialize environment for current app
RUN mkdir -pv /app
WORKDIR /app
COPY . .

# Install dependencies with lates nuget
RUN wget -O nuget.exe https://dist.nuget.org/win-x86-commandline/latest/nuget.exe && \
    mono nuget.exe restore && \
    rm nuget.exe

# Create protobuf files
RUN pwd
RUN pb/generate.sh

# Compile app
RUN xbuild

ENTRYPOINT ["mono", "/app/ModbusImp.Service/bin/Debug/ModbusImp.Service.exe"]

EXPOSE 50051
