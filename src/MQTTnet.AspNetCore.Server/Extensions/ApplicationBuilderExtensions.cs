using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.Server;

namespace MQTTnet.AspNetCore.Server.Extensions;

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
}