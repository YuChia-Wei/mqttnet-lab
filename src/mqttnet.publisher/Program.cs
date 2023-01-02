using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using MQTTnet.Client;
using mqttnet.subscriber.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMqttClient>(_ => new MqttFactory().CreateMqttClient());
builder.Services.AddHostedService<MqttClientBackgroundService>();

var app = builder.Build();

app.MapPut("/publish", async (
               [FromServices] IMqttClient mqttClient,
               [FromBody] MqttData mqttData,
               CancellationToken stoppingToken) =>
           {
               var applicationMessage = new MqttApplicationMessageBuilder()
                                        .WithTopic(mqttData.TopicId)
                                        .WithPayload(mqttData.Data)
                                        .Build();

               await mqttClient.PublishAsync(applicationMessage, stoppingToken);
           });

app.Run();

public record MqttData
{
    public string Data { get; set; }
    public string TopicId { get; set; }
}