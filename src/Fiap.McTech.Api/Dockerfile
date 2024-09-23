FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app
EXPOSE 8080
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

RUN addgroup --group mctech --gid 2000 \
&& adduser \    
    --uid 1000 \
    --gid 2000 \
    "mctech" 

RUN chown mctech:mctech  /app /tmp
USER mctech:mctech

ENTRYPOINT ["dotnet", "Fiap.McTech.Api.dll"]
