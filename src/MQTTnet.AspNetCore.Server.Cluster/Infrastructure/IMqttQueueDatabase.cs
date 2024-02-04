using System.Threading.Tasks;
using MQTTnet.Server;

namespace MQTTnet.AspNetCore.Server.Cluster.Infrastructure;

internal interface IMqttQueueDatabase
{
    /// <summary>
    /// Publish to other broker
    /// </summary>
    /// <param name="eventArgs"></param>
    /// <returns></returns>
    Task PublishAsync(InterceptingPublishEventArgs eventArgs);
}