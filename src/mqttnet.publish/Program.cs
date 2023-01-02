using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using MQTTnet.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMqttClient>(_ => new MqttFactory().CreateMqttClient());

var app = builder.Build();

app.MapPut("/publish", async (
               [FromServices] IMqttClient mqttClient,
               [FromBody] MqttData mqttData,
               CancellationToken stoppingToken) =>
           {
               var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("localhost").Build();
               await mqttClient.ConnectAsync(mqttClientOptions, stoppingToken);

               var applicationMessage = new MqttApplicationMessageBuilder()
                                        .WithTopic(mqttData.TopicId)
                                        .WithPayload(mqttData.Data)
                                        .Build();

               await mqttClient.PublishAsync(applicationMessage, stoppingToken);

               await mqttClient.DisconnectAsync();
           });

app.Run();

public record MqttData
{
    public string Data { get; set; }
    public string TopicId { get; set; }
}