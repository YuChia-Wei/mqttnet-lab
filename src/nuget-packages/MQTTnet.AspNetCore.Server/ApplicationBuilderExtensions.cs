using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.Server;

namespace MQTTnet.AspNetCore.Server;

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
    /// Append Client Subscribed Topic Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseClientSubscribedTopicEvent(
        this IApplicationBuilder app,
        Func<ClientSubscribedTopicEventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();
        server.ClientSubscribedTopicAsync += eventFunc;
        return app;
    }

    /// <summary>
    /// Append Client Unsubscribed Topic Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseClientUnsubscribedTopicEvent(
        this IApplicationBuilder app,
        Func<ClientUnsubscribedTopicEventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();
        server.ClientUnsubscribedTopicAsync += eventFunc;
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
    /// Append Intercepting Subscription Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseInterceptingSubscriptionEvent(
        this IApplicationBuilder app,
        Func<InterceptingSubscriptionEventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();
        server.InterceptingSubscriptionAsync += eventFunc;
        return app;
    }

    /// <summary>
    /// Append Intercepting Unsubscription Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseInterceptingUnsubscriptionEvent(
        this IApplicationBuilder app,
        Func<InterceptingUnsubscriptionEventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();
        server.InterceptingUnsubscriptionAsync += eventFunc;
        return app;
    }

    /// <summary>
    /// Append Loading Retained Messages Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseLoadingRetainedMessagesEvent(
        this IApplicationBuilder app,
        Func<LoadingRetainedMessagesEventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();
        server.LoadingRetainedMessageAsync += eventFunc;
        return app;
    }

    public static IApplicationBuilder UseMqttServerEventHandler(
        this IApplicationBuilder app)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();
        // server.LoadingRetainedMessageAsync += eventFunc;
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

    /// <summary>
    /// Append Preparing Session Event (maybe error?)
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    internal static IApplicationBuilder UsePreparingSessionEvent(
        this IApplicationBuilder app,
        Func<PreparingSessionEventArgs, Task> eventFunc)
    {
        //TODO: 官方套件的 PreparingSessionAsync 並不是接 PreparingSessionEventArgs，而是 EventArgs
        // var server = app.ApplicationServices.GetRequiredService<MqttServer>();
        // server.PreparingSessionAsync += eventFunc;
        return app;
    }

    /// <summary>
    /// Append Preparing Session Event (maybe error?)
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    internal static IApplicationBuilder UsePreparingSessionEvent(
        this IApplicationBuilder app,
        Func<EventArgs, Task> eventFunc)
    {
        //TODO: 官方套件的 PreparingSessionAsync 並不是接 PreparingSessionEventArgs，而是 EventArgs
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();
        server.PreparingSessionAsync += eventFunc;
        return app;
    }

    /// <summary>
    /// Append Retained Message Changed Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    internal static IApplicationBuilder UseRetainedMessageChangedEvent(
        this IApplicationBuilder app,
        Func<RetainedMessageChangedEventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();
        server.RetainedMessageChangedAsync += eventFunc;
        return app;
    }

    /// <summary>
    /// Append Retained Message Cleared Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    internal static IApplicationBuilder UseRetainedMessagesClearedEvent(
        this IApplicationBuilder app,
        Func<EventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();
        server.RetainedMessagesClearedAsync += eventFunc;
        return app;
    }

    /// <summary>
    /// Append Session Deleted Event
    /// </summary>
    /// <param name="app"></param>
    /// <param name="eventFunc"></param>
    /// <returns></returns>
    internal static IApplicationBuilder UseSessionDeletedEvent(
        this IApplicationBuilder app,
        Func<SessionDeletedEventArgs, Task> eventFunc)
    {
        var server = app.ApplicationServices.GetRequiredService<MqttServer>();
        server.SessionDeletedAsync += eventFunc;
        return app;
    }
}