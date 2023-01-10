using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.AspNetCore.Server.Cluster.Events;
using MQTTnet.Server;

namespace MQTTnet.AspNetCore.Server.Cluster.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMqttClusterQueueRedisDb(this IApplicationBuilder app)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();

        var publishEvents = app.ApplicationServices.GetRequiredService<InterceptingPublishEvents>();

        server.InterceptingPublishAsync += publishEvents.PublishToMqttClusterQueueDatabase;

        return app;
    }
}