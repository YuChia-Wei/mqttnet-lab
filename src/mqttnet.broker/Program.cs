using MQTTnet.AspNetCore;
using mqttnet.broker.Events;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    // link = mqtt://{host}:1883
    options.ListenAnyIP(1883, listenOptions => listenOptions.UseMqtt());
    // UseMqtt 實際上是下面這行
    // options.ListenAnyIP(1883, listenOptions =>listenOptions.UseConnectionHandler<MqttConnectionHandler>());

    // link = wss://{host}:64430
    options.ListenAnyIP(64430, listenOptions => listenOptions.UseHttps());

    // link = ws://{host}:4430
    options.ListenAnyIP(4430);
});

builder.Services.AddMqttServer();
// AddMqttServer = 以下兩行
// builder.Services.AddMqttConnectionHandler();
// builder.Services.AddHostedMqttServer();

builder.Services.AddConnections();

builder.Services.AddSingleton<MqttEvents>();

var app = builder.Build();

app.UseRouting();

//ws / wss route
app.MapMqtt("/mqtt");

//default route = mqtt://{host}:1883
app.UseMqttServer(server =>
{
    var requiredService = app.Services.GetRequiredService<MqttEvents>();

    server.InterceptingPublishAsync += requiredService.OnInterceptingPublish;
    server.ClientConnectedAsync += requiredService.OnClientConnected;
    server.ClientDisconnectedAsync += requiredService.OnClientDisconnected;
    server.ValidatingConnectionAsync += requiredService.ValidateConnection;
});

app.Run();