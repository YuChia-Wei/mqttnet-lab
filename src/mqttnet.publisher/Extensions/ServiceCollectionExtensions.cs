using Microsoft.Extensions.DependencyInjection.Extensions;
using MQTTnet.Client;

namespace mqttnet.publisher.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMqttClientOptions(
        this IServiceCollection serviceCollection,
        Action<IServiceProvider, MqttClientOptionsBuilder>? optionsAction)
    {
        serviceCollection.TryAddSingleton<MqttClientOptions>(serviceProvider =>
                                                                 CreateMqttClientOptions(
                                                                     serviceProvider, optionsAction));
        return serviceCollection;
    }

    private static MqttClientOptions CreateMqttClientOptions(
        IServiceProvider applicationServiceProvider,
        Action<IServiceProvider, MqttClientOptionsBuilder>? optionsAction)
    {
        var builder = new MqttClientOptionsBuilder();

        optionsAction?.Invoke(applicationServiceProvider, builder);

        return builder.Build();
    }
}