FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["bot-discord-hubla/bot-discord-hubla.csproj", "bot-discord-hubla/"]
RUN dotnet restore "bot-discord-hubla/bot-discord-hubla.csproj"
COPY . .
WORKDIR "/src/bot-discord-hubla"
RUN dotnet build "./bot-discord-hubla.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./bot-discord-hubla.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "bot-discord-hubla.dll"]
