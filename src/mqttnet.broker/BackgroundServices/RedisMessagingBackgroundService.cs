using System.Text.Json;
using mqttnet.broker.Events;
using MQTTnet.Server;
using StackExchange.Redis;

namespace mqttnet.broker.BackgroundServices;

/// <summary>
/// Mqtt Client 背景連線服務，讓這個 asp net core server 建立訂閱
/// </summary>
public class RedisMessagingBackgroundService : BackgroundService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger<RedisMessagingBackgroundService> _logger;
    private readonly MqttServer _mqttServer;

    public RedisMessagingBackgroundService(
        IConnectionMultiplexer connectionMultiplexer,
        ILogger<RedisMessagingBackgroundService> logger,
        MqttServer mqttServer)
    {
        this._connectionMultiplexer = connectionMultiplexer;
        this._logger = logger;
        this._mqttServer = mqttServer;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public override void Dispose()
    {
        this._connectionMultiplexer.Dispose();
        base.Dispose();
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
                var mqttSyncData = JsonSerializer.Deserialize<MqttSyncData>(message);

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

                var injectedMqttApplicationMessage = new InjectedMqttApplicationMessage(mqttSyncData.ApplicationMessage)
                {
                    SenderClientId = mqttSyncData.OriginPublisher
                };
                this._mqttServer.InjectApplicationMessage(injectedMqttApplicationMessage, stoppingToken);
                
                this._logger.LogInformation((string)message ?? "");
            });

        stoppingToken.Register(
            () => this._logger.LogInformation($"{AppDomain.CurrentDomain.FriendlyName} background task is stopping."));

        return Task.CompletedTask;
    }
}