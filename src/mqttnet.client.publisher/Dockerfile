FROM ghcr.io/yuchia-wei/otel-dotnet-auto-instrumentation:8.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["mqttnet.client.publisher/mqttnet.client.publisher.csproj", "mqttnet.client.publisher/"]
COPY ["MQTTnet.AspNetCore.Client.BackgroundServices/MQTTnet.AspNetCore.Client.BackgroundServices.csproj", "MQTTnet.AspNetCore.Client.BackgroundServices/"]
COPY ["MQTTnet.AspNetCore.Client.DependencyInjection/MQTTnet.AspNetCore.Client.DependencyInjection.csproj", "MQTTnet.AspNetCore.Client.DependencyInjection/"]
RUN dotnet restore "mqttnet.client.publisher/mqttnet.client.publisher.csproj"
COPY . .
WORKDIR "/src/mqttnet.client.publisher"
RUN dotnet build "mqttnet.client.publisher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "mqttnet.client.publisher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mqttnet.client.publisher.dll"]
