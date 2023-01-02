FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["mqtt.publish/mqtt.publish.csproj", "mqtt.publish/"]
RUN dotnet restore "mqtt.publish/mqtt.publish.csproj"
COPY . .
WORKDIR "/src/mqtt.publish"
RUN dotnet build "mqtt.publish.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "mqtt.publish.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mqtt.publish.dll"]
