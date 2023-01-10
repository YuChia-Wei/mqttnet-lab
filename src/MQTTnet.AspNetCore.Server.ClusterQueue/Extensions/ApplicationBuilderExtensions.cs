﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.AspNetCore.Server.ClusterQueue.Events;
using MQTTnet.Server;

namespace MQTTnet.AspNetCore.Server.ClusterQueue.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseInterceptingPublishEvent(
        this IApplicationBuilder app,
        Func<InterceptingPublishEventArgs, Task> serverOnInterceptingPublishAsync)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();

        server.InterceptingPublishAsync += serverOnInterceptingPublishAsync;

        return app;
    }

    public static IApplicationBuilder UseMqttClusterQueueRedisDb(this IApplicationBuilder app)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();

        var publishEvents = app.ApplicationServices.GetRequiredService<InterceptingPublishEvents>();

        server.InterceptingPublishAsync += publishEvents.PublishToMqttClusterQueueDatabase;

        return app;
    }
}