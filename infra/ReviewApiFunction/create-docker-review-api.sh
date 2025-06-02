# 1. 🧹 Rensa ev. gammal image
docker rmi -f reviewapi-gbgmerch

# 2. ✍️ Skapa rätt Dockerfile
echo "FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/azure-functions/dotnet:4
WORKDIR /home/site/wwwroot
COPY --from=build /app ." > Dockerfile

# 3. 🏗 Bygg Docker-imagen
docker build -t reviewapi-gbgmerch .

# 4. 🚀 Kör containern på port 7073
docker run -it -p 7073:80 reviewapi-gbgmerch
