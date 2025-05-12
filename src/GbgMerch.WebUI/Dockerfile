FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Kopiera csproj-filer för alla projekt (relativa till src/)
COPY GbgMerch.Domain/GbgMerch.Domain.csproj ./GbgMerch.Domain/
COPY GbgMerch.Application/GbgMerch.Application.csproj ./GbgMerch.Application/
COPY GbgMerch.Infrastructure/GbgMerch.Infrastructure.csproj ./GbgMerch.Infrastructure/
COPY GbgMerch.WebUI/GbgMerch.WebUI.csproj ./GbgMerch.WebUI/

# Återställ beroenden
RUN dotnet restore ./GbgMerch.WebUI/GbgMerch.WebUI.csproj

# Kopiera hela src/
COPY . .

# Publicera
WORKDIR /src/GbgMerch.WebUI
RUN dotnet publish "GbgMerch.WebUI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GbgMerch.WebUI.dll"]
