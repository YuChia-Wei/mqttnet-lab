using MQTTnet;
using MQTTnet.Client;
using mqttnet.client.subscriber.Back;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MqttFactory>();

builder.Services.AddHostedService<MqttClientBackgroundService>();

var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.Run();