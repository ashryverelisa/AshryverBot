FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY AshrverBot.sln ./
COPY AshryverBot.Database/AshryverBot.Database.csproj AshryverBot.Database/
COPY AshryverBot.Infrastructure/AshryverBot.Infrastructure.csproj AshryverBot.Infrastructure/
COPY AshryverBot.Twitch/AshryverBot.Twitch.csproj AshryverBot.Twitch/
COPY AshryverBot.Web/AshryverBot.Web.csproj AshryverBot.Web/

RUN dotnet restore AshryverBot.Web/AshryverBot.Web.csproj

COPY . .

RUN dotnet publish AshryverBot.Web/AshryverBot.Web.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

USER app

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 8080

COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "AshryverBot.Web.dll"]
