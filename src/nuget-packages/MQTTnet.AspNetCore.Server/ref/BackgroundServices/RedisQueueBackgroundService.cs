using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet.AspNetCore.Server.@ref.Entities;
using MQTTnet.Server;
using StackExchange.Redis;

namespace MQTTnet.AspNetCore.Server.@ref.BackgroundServices;

/// <summary>
/// Mqtt Client 背景連線服務，讓這個 asp net core server 建立訂閱
/// </summary>
public class RedisQueueBackgroundService : BackgroundService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger<RedisQueueBackgroundService> _logger;
    private readonly MqttServer _mqttServer;

    public RedisQueueBackgroundService(
        IConnectionMultiplexer connectionMultiplexer,
        ILogger<RedisQueueBackgroundService> logger,
        MqttServer mqttServer)
    {
        this._connectionMultiplexer = connectionMultiplexer;
        this._logger = logger;
        this._mqttServer = mqttServer;
    }

    /// <summary>
    /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation
    /// should return a task that represents
    /// the lifetime of the long running operation(s) being performed.
    /// </summary>
    /// <param name="stoppingToken">
    /// Triggered when
    /// <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.
    /// </param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the long running operations.</returns>
    /// <remarks>
    /// See <see href="https://docs.microsoft.com/dotnet/core/extensions/workers">Worker Services in .NET</see> for
    /// implementation guidelines.
    /// </remarks>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this._connectionMultiplexer.GetSubscriber()
            .SubscribeAsync($"{AppDomain.CurrentDomain.FriendlyName}:MqttSync", (channel, message) =>
            {
                var mqttSyncData = MqttClusterQueueEntity.Deserialize(message);

                if (mqttSyncData == null)
                {
                    this._logger.LogWarning("MqttSyncData is null");
                    return;
                }

                if (mqttSyncData.IsSameBroker())
                {
                    this._logger.LogInformation("This is my message!");
                    return;
                }

                this.PublishFromBroker(mqttSyncData, stoppingToken);
            });

        stoppingToken.Register(
            () => this._logger.LogInformation($"{AppDomain.CurrentDomain.FriendlyName} background task is stopping."));

        return Task.CompletedTask;
    }

    private void PublishFromBroker(MqttClusterQueueEntity mqttClusterQueueEntity, CancellationToken stoppingToken)
    {
        var applicationMessage = mqttClusterQueueEntity.CreateInjectedMqttApplicationMessage();

        this._mqttServer.InjectApplicationMessage(applicationMessage, stoppingToken);

        this._logger.LogInformation(
            $"SenderClient '{applicationMessage.SenderClientId}' " +
            $"publish sync topic: {applicationMessage.ApplicationMessage.Topic}");
    }
}