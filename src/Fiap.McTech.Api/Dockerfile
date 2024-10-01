FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN groupadd -r mctech && useradd -r -g mctech mctech
WORKDIR /app
EXPOSE 8081
EXPOSE 443
ARG ALLOW_SWAGGER_UI=false
ARG ALLOW_ORIGINS=*
COPY ["src/Fiap.McTech.Api/cert.pfx", "/app"]

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Fiap.McTech.Api", "Fiap.McTech.Api"]
COPY ["src/Fiap.McTech.Application", "Fiap.McTech.Application"]
COPY ["src/Fiap.McTech.Domain", "Fiap.McTech.Domain"]
COPY ["src/Fiap.McTech.Infra", "Fiap.McTech.Infra"]
COPY ["src/Fiap.McTech.CrossCutting.Ioc", "Fiap.McTech.CrossCutting.Ioc"]
COPY ["src/Fiap.McTech.Services", "Fiap.McTech.Services"]
RUN dotnet restore "./Fiap.McTech.Api/Fiap.McTech.Api.csproj"
WORKDIR "/src/Fiap.McTech.Api"
RUN dotnet build "./Fiap.McTech.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Fiap.McTech.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER mctech
ENTRYPOINT ["dotnet", "Fiap.McTech.Api.dll"]
