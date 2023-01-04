FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443
EXPOSE 64430
EXPOSE 4430
EXPOSE 1883

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["mqttnet.broker.csproj", "./"]
RUN dotnet restore "mqttnet.broker.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "mqttnet.broker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "mqttnet.broker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mqttnet.broker.dll"]
