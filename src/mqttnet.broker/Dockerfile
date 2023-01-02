FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443
EXPOSE 64430
EXPOSE 4430
EXPOSE 1883

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["mqtt.broker.csproj", "./"]
RUN dotnet restore "mqtt.broker.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "mqtt.broker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "mqtt.broker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mqtt.broker.dll"]
