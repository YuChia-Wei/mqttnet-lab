using MQTTnet;
using MQTTnet.Client;

namespace mqttnet.subscriber.BackgroundServices;

public class MqttClientBackgroundService : BackgroundService
{
    private IMqttClient _mqttClient;

    public MqttClientBackgroundService(IMqttClient mqttClient)
    {
        this._mqttClient = mqttClient;
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        this._mqttClient.DisconnectAsync();
        this._mqttClient.Dispose();

        return base.StopAsync(cancellationToken);
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
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var mqttClientOptions = new MqttClientOptionsBuilder()
                                .WithTcpServer("localhost")
                                .Build();

        this._mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            Console.WriteLine($"{e.ApplicationMessage.Topic}:{e.ApplicationMessage.ConvertPayloadToString()}");
            return Task.CompletedTask;
        };

        await this._mqttClient.ConnectAsync(mqttClientOptions, stoppingToken);

        var mqttSubscribeOptions = new MqttClientSubscribeOptionsBuilder()
                                   .WithTopicFilter(mqttTopicFilterBuilder =>
                                   {
                                       mqttTopicFilterBuilder.WithTopic("test");
                                   })
                                   .Build();

        await this._mqttClient.SubscribeAsync(mqttSubscribeOptions);
    }
}