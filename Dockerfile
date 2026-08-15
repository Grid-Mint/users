# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /src

# Спочатку тільки файли проєктів — шар кешується, поки вони не змінились
COPY Users.slnx ./
COPY src/Domain/Users.Domain.csproj src/Domain/
COPY src/Application/Users.Application.csproj src/Application/
COPY src/Infrastructure/Users.Infrastructure.csproj src/Infrastructure/
COPY src/Api/Users.Api.csproj src/Api/
RUN dotnet restore src/Api/Users.Api.csproj

COPY src/ src/
RUN dotnet publish src/Api/Users.Api.csproj -c Release -o /app --no-restore

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app
COPY --from=builder /app .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

USER app

ENTRYPOINT ["dotnet", "Users.Api.dll"]
