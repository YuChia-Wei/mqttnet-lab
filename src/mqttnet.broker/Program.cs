using MQTTnet;
using MQTTnet.AspNetCore;
using MQTTnet.AspNetCore.Server;
using MQTTnet.AspNetCore.Server.Cluster.Extensions;
using mqttnet.broker.Events;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    // link = mqtt://{host}
    options.ListenAnyIP(1883, listenOptions => listenOptions.UseMqtt());
    // UseMqtt 實際上是下面這行
    // options.ListenAnyIP(1883, listenOptions =>listenOptions.UseConnectionHandler<MqttConnectionHandler>());

    // link = wss://{host}:443
    // need ssl certificates
    // options.ListenAnyIP(443, listenOptions => listenOptions.UseHttps());

    // link = ws://{host}:80
    options.ListenAnyIP(80);
    options.ListenAnyIP(8080);
});

builder.Services.AddMqttServer();

builder.Services.AddMqttClusterQueueRedisDb(o =>
{
    o.RedisConnectionString = Environment.GetEnvironmentVariable("RedisConnection")
                              ?? builder.Configuration.GetValue<string>("RedisConnection");
});

var app = builder.Build();

app.UseRouting();

//ws / wss route
app.MapMqtt("/mqtt");

//default route = mqtt://{host}:1883
app.UseMqttServer(server =>
{
    var requiredService = app.Services.GetRequiredService<MqttEvents>();
    server.ClientConnectedAsync += requiredService.OnClientConnectedAsync;
    server.ClientDisconnectedAsync += requiredService.OnClientDisconnectedAsync;
    server.ValidatingConnectionAsync += requiredService.ValidateConnectionAsync;
});

app.UseInterceptingPublishEvent(eventArgs =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();

    logger.LogInformation(
        $"[Middleware Interception]\n Client '{eventArgs.ClientId}' publish: {eventArgs.ApplicationMessage.Topic}:{eventArgs.ApplicationMessage.ConvertPayloadToString()}");

    return Task.CompletedTask;
});

app.UseRedisMqttServerCluster();

app.Run();