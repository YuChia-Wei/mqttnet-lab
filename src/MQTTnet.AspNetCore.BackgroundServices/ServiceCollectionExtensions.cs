using Microsoft.Extensions.DependencyInjection;

namespace MQTTnet.AspNetCore.BackgroundServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMqttBackgroundConnectService(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<MqttClientBackgroundService>();
        return serviceCollection;
    }
}