#build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY WebApi/Otello.WebApi.csproj Otello.WebApi.csproj  
RUN dotnet restore

COPY . .

RUN dotnet build Otello.WebApi.csproj --configuration Release --output /app/build

# serve
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app

RUN groupadd -g 10000 dotnet && useradd -u 10000 -g dotnet dotnet && chown -R dotnet:dotnet /app
USER dotnet:dotnet

ENV ASPNETCORE_URLS=http://*:5080
EXPOSE 5080

COPY --chown=dotnet:dotnet --from=build /app/build .
COPY --chown=dotnet:dotnet --from=build /app/build .

ENTRYPOINT ["dotnet", "Otello.WebApi.dll"]