using Microsoft.Extensions.DependencyInjection;

namespace MQTTnet.AspNetCore.Server;

/// <summary>
/// 參考 IDataProtectionBuilder 等其他官方套件的 Builder 寫法
/// 原本想參考 AuthenticationBuilder，但是覺得好像用介面比較好
/// </summary>
public interface IMqttServerBuilder
{
    IServiceCollection Services { get; }
}