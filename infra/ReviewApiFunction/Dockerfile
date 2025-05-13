FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/azure-functions/dotnet:4
WORKDIR /home/site/wwwroot
COPY --from=build /app .
