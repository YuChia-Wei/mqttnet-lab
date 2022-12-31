FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["mqtt.subscribe/mqtt.subscribe.csproj", "mqtt.subscribe/"]
RUN dotnet restore "mqtt.subscribe/mqtt.subscribe.csproj"
COPY . .
WORKDIR "/src/mqtt.subscribe"
RUN dotnet build "mqtt.subscribe.csproj" -c Release -o /app/build

FROM build AS subscribe
RUN dotnet subscribe "mqtt.subscribe.csproj" -c Release -o /app/subscribe

FROM base AS final
WORKDIR /app
COPY --from=subscribe /app/subscribe .
ENTRYPOINT ["dotnet", "mqtt.subscribe.dll"]
