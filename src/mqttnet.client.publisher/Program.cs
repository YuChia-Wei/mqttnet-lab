using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using MQTTnet.AspNetCore.Client;
using MQTTnet.AspNetCore.Client.DependencyInjection;
using MQTTnet.Client;
using mqttnet.client.publisher;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMqttClient((_, clientOptionBuilder) =>
{
    clientOptionBuilder.WithTcpServer(Environment.GetEnvironmentVariable("broker"))
                       .WithClientId(Environment.MachineName);
                       // .WithClientId(AppDomain.CurrentDomain.FriendlyName);
    //will error
    // .WithProtocolVersion(MqttProtocolVersion.Unknown)
    //is default
    // .WithProtocolVersion(MqttProtocolVersion.V311)
    //if need to change
    // .WithProtocolVersion(MqttProtocolVersion.V310)
    // .WithProtocolVersion(MqttProtocolVersion.V500)
});

builder.Services.AddMqttClientBackgroundService();

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

namespace mqttnet.client.publisher
{
    public record MqttData
    {
        public string Data { get; set; }
        public string TopicId { get; set; }
    }
}