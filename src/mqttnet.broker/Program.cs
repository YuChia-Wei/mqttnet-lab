using MQTTnet.AspNetCore;
using MQTTnet.AspNetCore.Server.ClusterQueue.Extensions;
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
});

builder.Services.AddMqttServer();
// AddMqttServer = 以下兩行
// builder.Services.AddMqttConnectionHandler();
// builder.Services.AddHostedMqttServer();

builder.Services.AddConnections();

builder.Services.AddSingleton<MqttEvents>();

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
    server.ClientConnectedAsync += requiredService.OnClientConnected;
    server.ClientDisconnectedAsync += requiredService.OnClientDisconnected;
    server.ValidatingConnectionAsync += requiredService.ValidateConnection;
});

app.UseMqttClusterQueueRedisDb();

app.Run();