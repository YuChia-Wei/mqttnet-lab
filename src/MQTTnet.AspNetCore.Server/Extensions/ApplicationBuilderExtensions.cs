using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.Server;

namespace MQTTnet.AspNetCore.Server.Extensions;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Append Application Message Not Consumed Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseApplicationMessageNotConsumedEvent(
        this IApplicationBuilder app,
        Func<ApplicationMessageNotConsumedEventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();

        server.ApplicationMessageNotConsumedAsync += eventFunc;

        return app;
    }

    /// <summary>
    /// Append Client Acknowledged Publish Packet Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseClientAcknowledgedPublishPacketEvent(
        this IApplicationBuilder app,
        Func<ClientAcknowledgedPublishPacketEventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();

        server.ClientAcknowledgedPublishPacketAsync += eventFunc;

        return app;
    }

    /// <summary>
    /// Append Client Connected Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseClientConnectedEvent(
        this IApplicationBuilder app,
        Func<ClientConnectedEventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();

        server.ClientConnectedAsync += eventFunc;

        return app;
    }

    /// <summary>
    /// Append Client Disconnected Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseClientDisconnectedEvent(
        this IApplicationBuilder app,
        Func<ClientDisconnectedEventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();

        server.ClientDisconnectedAsync += eventFunc;

        return app;
    }

    /// <summary>
    /// Append Intercepting Inbound Packet Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseInterceptingInboundPacketEvent(
        this IApplicationBuilder app,
        Func<InterceptingPacketEventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();

        server.InterceptingInboundPacketAsync += eventFunc;

        return app;
    }

    /// <summary>
    /// Append Intercepting Outbound Packet Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseInterceptingOutboundPacketEvent(
        this IApplicationBuilder app,
        Func<InterceptingPacketEventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();

        server.InterceptingOutboundPacketAsync += eventFunc;

        return app;
    }

    /// <summary>
    /// Append Interception Publish Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseInterceptingPublishEvent(
        this IApplicationBuilder app,
        Func<InterceptingPublishEventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();

        server.InterceptingPublishAsync += eventFunc;

        return app;
    }

    /// <summary>
    /// Append Validating Connection Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseValidatingConnectionEvent(
        this IApplicationBuilder app,
        Func<ValidatingConnectionEventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();

        server.ValidatingConnectionAsync += eventFunc;

        return app;
    }
}