FROM ghcr.io/yuchia-wei/otel-dotnet-auto-instrumentation:8.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["mqttnet.client.subscriber/mqttnet.client.subscriber.csproj", "mqttnet.client.subscriber/"]
COPY ["MQTTnet.AspNetCore.Client.BackgroundServices/MQTTnet.AspNetCore.Client.BackgroundServices.csproj", "MQTTnet.AspNetCore.Client.BackgroundServices/"]
COPY ["MQTTnet.AspNetCore.Client.DependencyInjection/MQTTnet.AspNetCore.Client.DependencyInjection.csproj", "MQTTnet.AspNetCore.Client.DependencyInjection/"]
RUN dotnet restore "mqttnet.client.subscriber/mqttnet.client.subscriber.csproj"
COPY . .
WORKDIR "/src/mqttnet.client.subscriber"
RUN dotnet build "mqttnet.client.subscriber.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "mqttnet.client.subscriber.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mqttnet.client.subscriber.dll"]
