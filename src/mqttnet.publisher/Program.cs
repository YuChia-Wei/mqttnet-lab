using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using MQTTnet.AspNetCore.BackgroundServices;
using MQTTnet.AspNetCore.DependencyInjection;
using MQTTnet.Client;
using mqttnet.publisher;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMqttClient((_, clientOptionBuilder) =>
{
    clientOptionBuilder.WithTcpServer("localhost")
                       .WithClientId(AppDomain.CurrentDomain.FriendlyName);
    //will error
    // .WithProtocolVersion(MqttProtocolVersion.Unknown)
    //is default
    // .WithProtocolVersion(MqttProtocolVersion.V311)
    //if need to change
    // .WithProtocolVersion(MqttProtocolVersion.V310)
    // .WithProtocolVersion(MqttProtocolVersion.V500)
});

builder.Services.AddMqttBackgroundConnectService();

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

namespace mqttnet.publisher
{
    public record MqttData
    {
        public string Data { get; set; }
        public string TopicId { get; set; }
    }
}