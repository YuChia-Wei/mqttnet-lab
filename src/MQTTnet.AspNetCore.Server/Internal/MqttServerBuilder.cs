using Microsoft.Extensions.DependencyInjection;

namespace MQTTnet.AspNetCore.Server.Internal;

/// <summary>
/// Default Implementation of <see cref="IMqttServerBuilder" />>
/// </summary>
internal class MqttServerBuilder : IMqttServerBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IServiceCollection" />
    /// </summary>
    public MqttServerBuilder(IServiceCollection services)
    {
        this.Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public IServiceCollection Services { get; }
}