using MQTTnet;
using MQTTnet.Client;
using mqttnet.subscriber.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMqttClient>(_ => new MqttFactory().CreateMqttClient());
builder.Services.AddHostedService<MqttClientBackgroundService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();