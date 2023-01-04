FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["mqttnet.publisher.csproj", "./"]
RUN dotnet restore "mqttnet.publisher.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "mqttnet.publisher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "mqttnet.publisher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mqttnet.publisher.dll"]
