using System.Threading.Tasks;
using MQTTnet.Server;

namespace MQTTnet.AspNetCore.Server.ClusterQueue.Infrastructure;

internal interface IMqttClusterQueueDatabase
{
    /// <summary>
    /// Publish to other broker
    /// </summary>
    /// <param name="eventArgs"></param>
    /// <returns></returns>
    Task PublishAsync(InterceptingPublishEventArgs eventArgs);
}