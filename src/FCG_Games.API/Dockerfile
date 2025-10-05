FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FCG_Games.API/FCG_Games.API.csproj", "src/FCG_Games.API/"]
COPY ["src/FCG_Games.Application/FCG_Games.Application.csproj", "src/FCG_Games.Application/"]
COPY ["src/FCG_Games.Domain/FCG_Games.Domain.csproj", "src/Domain.API/"]
COPY ["src/FCG_Games.Infrastructure/FCG_Games.Infrastructure.csproj", "src/FCG_Games.Infrastructure/"]
RUN dotnet restore "src/FCG_Games.API/FCG_Games.API.csproj"
COPY src/ .
WORKDIR "/src/FCG_Games.API"
RUN dotnet build "./FCG_Games.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FCG_Games.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FCG_Games.API.dll"]