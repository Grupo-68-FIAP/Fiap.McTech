FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY ./src/Fiap.McTech.Api/cert.pfx /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Fiap.McTech.Api/Fiap.McTech.Api.csproj", "src/Fiap.McTech.Api/"]
COPY ["src/Fiap.McTech.Application/Fiap.McTech.Application.csproj", "src/Fiap.McTech.Application/"]
COPY ["src/Fiap.McTech.Domain/Fiap.McTech.Domain.csproj", "src/Fiap.McTech.Domain/"]
COPY ["src/Fiap.McTech.Infra/Fiap.McTech.Infra.csproj", "src/Fiap.McTech.Infra/"]
COPY ["src/Fiap.McTech.CrossCutting.Ioc/Fiap.McTech.CrossCutting.Ioc.csproj", "src/Fiap.McTech.CrossCutting.Ioc/"]
RUN dotnet restore "./src/Fiap.McTech.Api/Fiap.McTech.Api.csproj"
COPY . .
WORKDIR "/src/src/Fiap.McTech.Api"
RUN dotnet build "./Fiap.McTech.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Fiap.McTech.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fiap.McTech.Api.dll"]