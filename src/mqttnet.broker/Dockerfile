﻿FROM ghcr.io/yuchia-wei/otel-dotnet-auto-instrumentation:8.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8080
EXPOSE 1883

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["mqttnet.broker/mqttnet.broker.csproj", "mqttnet.broker/"]
COPY ["MQTTnet.AspNetCore.Server.ClusterQueue/MQTTnet.AspNetCore.Server.ClusterQueue.csproj", "MQTTnet.AspNetCore.Server.ClusterQueue/"]
RUN dotnet restore "mqttnet.broker/mqttnet.broker.csproj"
COPY . .
WORKDIR "/src/mqttnet.broker"
RUN dotnet build "mqttnet.broker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "mqttnet.broker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mqttnet.broker.dll"]
