using MQTTnet;
using MQTTnet.Client;

namespace mqttnet.client.subscriber.Back;

public class MqttClientBackgroundService : BackgroundService
{
    private readonly ILogger<MqttClientBackgroundService> _logger;
    private readonly IMqttClient _mqttClient;
    private readonly MqttFactory _mqttFactory;

    public MqttClientBackgroundService(ILogger<MqttClientBackgroundService> logger, MqttFactory mqttFactory)
    {
        this._logger = logger;
        this._mqttFactory = mqttFactory;
        this._mqttClient = mqttFactory.CreateMqttClient();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            await this.NormalDisconnection(cancellationToken);
        }

        await this.ServerShuttingDown(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await this.ConnectAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            var message = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fffzzz} 連接服務器失敗 Msg：{ex}";

            this._logger.LogError(ex, message);
        }
    }

    private async Task ConnectAsync(CancellationToken cancellationToken)
    {
        var mqttClientOptions = new MqttClientOptionsBuilder()
                                .WithTcpServer(Environment.GetEnvironmentVariable("broker"))
                                .WithClientId(Environment.MachineName)
                                .Build();

        await this._mqttClient.ConnectAsync(mqttClientOptions, cancellationToken);

        this._mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            Console.WriteLine("Received application message.");

            return Task.CompletedTask;
        };

        var mqttSubscribeOptions = this._mqttFactory.CreateSubscribeOptionsBuilder()
                                       .WithTopicFilter(f => f.WithTopic("samples/temperature/living_room"))
                                       .Build();

        await this._mqttClient.SubscribeAsync(mqttSubscribeOptions, cancellationToken);

        if (!this._mqttClient.IsConnected.Equals(false))
        {
            Console.WriteLine("IsConnected");
            return;
        }

        Console.WriteLine("retry");
        await this._mqttClient.ReconnectAsync(cancellationToken);
    }

    private async Task NormalDisconnection(CancellationToken cancellationToken)
    {
        var disconnectOption = new MqttClientDisconnectOptions()
        {
            Reason = MqttClientDisconnectReason.NormalDisconnection,
            ReasonString = "NormalDisconnection"
        };
        await this._mqttClient.DisconnectAsync(disconnectOption, cancellationToken);
    }

    private async Task ServerShuttingDown(CancellationToken cancellationToken)
    {
        var disconnectOption = new MqttClientDisconnectOptions()
        {
            Reason = MqttClientDisconnectReason.ServerShuttingDown,
            ReasonString = "ServerShuttingDown"
        };
        await this._mqttClient.DisconnectAsync(disconnectOption, cancellationToken);
    }
}