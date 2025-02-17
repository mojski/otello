#build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY WebApi/Otello.WebApi.csproj Otello.WebApi.csproj
RUN dotnet restore

COPY . .

RUN dotnet build WebApi/Otello.WebApi.csproj --configuration Release --output /app/build
RUN dotnet publish WebApi/Otello.WebApi.csproj --configuration Release --output /app/publish
# serve
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

WORKDIR /app

RUN groupadd -g 10000 dotnet && useradd -u 10000 -g dotnet dotnet && chown -R dotnet:dotnet /app
USER dotnet:dotnet

ENV ASPNETCORE_URLS http://*:5080
EXPOSE 5080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Otello.WebApi.dll"]