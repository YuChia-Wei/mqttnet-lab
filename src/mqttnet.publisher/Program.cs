using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MQTTnet;
using MQTTnet.Client;
using mqttnet.publisher;
using mqttnet.publisher.BackgroundServices;
using mqttnet.publisher.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.TryAddSingleton(new MqttFactory());
builder.Services.TryAddSingleton<IMqttClient>(provider => provider.GetRequiredService<MqttFactory>().CreateMqttClient());
builder.Services.AddMqttClientOptions((provider, clientOptionBuilder) =>
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

namespace mqttnet.publisher
{
    public record MqttData
    {
        public string Data { get; set; }
        public string TopicId { get; set; }
    }
}