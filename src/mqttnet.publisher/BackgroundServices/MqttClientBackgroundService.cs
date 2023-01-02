using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

namespace mqttnet.subscriber.BackgroundServices;

/// <summary>
/// Mqtt Client 背景連線服務，讓這個 asp net core server 建立訂閱
/// </summary>
public class MqttClientBackgroundService : BackgroundService
{
    private readonly IMqttClient _mqttClient;

    public MqttClientBackgroundService(IMqttClient mqttClient)
    {
        this._mqttClient = mqttClient;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public override void Dispose()
    {
        this._mqttClient.Dispose();
        base.Dispose();
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        this._mqttClient.DisconnectAsync();

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
                                .WithClientId(AppDomain.CurrentDomain.FriendlyName)
                                //will error
                                // .WithProtocolVersion(MqttProtocolVersion.Unknown)
                                //is default
                                // .WithProtocolVersion(MqttProtocolVersion.V311)
                                //if need to change
                                // .WithProtocolVersion(MqttProtocolVersion.V310)
                                // .WithProtocolVersion(MqttProtocolVersion.V500)
                                .Build();

        await this._mqttClient.ConnectAsync(mqttClientOptions, stoppingToken);
    }
}