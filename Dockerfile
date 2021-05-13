FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /app
COPY . .
RUN dotnet restore

# build api
WORKDIR /app
RUN dotnet build

# publish api
FROM build AS publish
RUN apk add dos2unix --update-cache --repository http://dl-3.alpinelinux.org/alpine/edge/community/ --allow-untrusted
WORKDIR /app
RUN dotnet publish -c Release -o out

# runtime container
FROM mcr.microsoft.com/dotnet/aspnet:3.1-alpine AS runtime
WORKDIR /app
COPY --from=publish /app/out ./
COPY --from=publish /app/entrypoint.sh ./
RUN chmod +x entrypoint.sh

ENTRYPOINT ["sh", "entrypoint.sh"]