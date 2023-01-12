using MQTTnet.Server;

namespace MQTTnet.AspNetCore.Server;

public interface IMqttServerEvents
{
    event Func<ApplicationMessageNotConsumedEventArgs, Task> ApplicationMessageNotConsumedAsync;
    event Func<ClientAcknowledgedPublishPacketEventArgs, Task> ClientAcknowledgedPublishPacketAsync;
    event Func<ClientConnectedEventArgs, Task> ClientConnectedAsync;
    event Func<ClientDisconnectedEventArgs, Task> ClientDisconnectedAsync;
    event Func<ClientSubscribedTopicEventArgs, Task> ClientSubscribedTopicAsync;
    event Func<ClientUnsubscribedTopicEventArgs, Task> ClientUnsubscribedTopicAsync;
    event Func<InterceptingPacketEventArgs, Task> InterceptingInboundPacketAsync;
    event Func<InterceptingPacketEventArgs, Task> InterceptingOutboundPacketAsync;
    event Func<InterceptingPublishEventArgs, Task> InterceptingPublishAsync;
    event Func<InterceptingSubscriptionEventArgs, Task> InterceptingSubscriptionAsync;
    event Func<InterceptingUnsubscriptionEventArgs, Task> InterceptingUnsubscriptionAsync;
    event Func<LoadingRetainedMessagesEventArgs, Task> LoadingRetainedMessageAsync;
    event Func<EventArgs, Task> PreparingSessionAsync;
    event Func<RetainedMessageChangedEventArgs, Task> RetainedMessageChangedAsync;
    event Func<EventArgs, Task> RetainedMessagesClearedAsync;
    event Func<SessionDeletedEventArgs, Task> SessionDeletedAsync;
    event Func<EventArgs, Task> StartedAsync;
    event Func<EventArgs, Task> StoppedAsync;
    event Func<ValidatingConnectionEventArgs, Task> ValidatingConnectionAsync;
}