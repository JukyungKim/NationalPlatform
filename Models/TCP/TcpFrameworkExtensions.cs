using System;
using System.Buffers;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Text;

namespace NationalPlatform.Models;

public static class TcpFrameworkExtensions
{
    public static IServiceCollection AddFramework(this IServiceCollection services, IPEndPoint endPoint)
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<KestrelServerOptions>, TcpFrameworkOptionsSetup>());

        services.Configure<TcpFrameworkOptions>(o =>
        {
            o.EndPoint = endPoint;
        });

        services.TryAddSingleton<IFrameworkMessageParser, FrameworkMessageParser>();
        return services;
    }
}

public class TcpFrameworkOptionsSetup : IConfigureOptions<KestrelServerOptions>
{
    private readonly TcpFrameworkOptions _options;

    public TcpFrameworkOptionsSetup(IOptions<TcpFrameworkOptions> options)
    {
        _options = options.Value;
    }

    public void Configure(KestrelServerOptions options)
    {
        options.Listen(_options.EndPoint, builder =>
        {
            builder.UseConnectionHandler<TcpFrameworkConnectionHandler>();
        });
    }

    // This is the connection handler the framework uses to handle new incoming connections
    private class TcpFrameworkConnectionHandler : ConnectionHandler
    {
        private readonly IFrameworkMessageParser _parser;

        public TcpFrameworkConnectionHandler(IFrameworkMessageParser parser)
        {
            _parser = parser;
        }

        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            Console.WriteLine("TcpFrameworkExtentions : Connected");

            var input = connection.Transport.Input;
            // Code to parse framework messages
            while (true)
            {
                var result = await input.ReadAsync();
                var buffer = result.Buffer;

                if(result.IsCompleted){
                    break;
                }

                Console.WriteLine("result:" + result);
                Console.WriteLine("buffer:" + buffer);

                if (_parser.TryParseMessage(ref buffer, out var message))
                {
                    await ProcessMessageAsync(message);
                }

                // input.AdvanceTo(buffer.Start, buffer.End);
                input.AdvanceTo(buffer.End);
            }
            Console.WriteLine("TcpFrameworkExtentions : Disconnected");
        }

        private Task ProcessMessageAsync(Message message)
        {
            Action<object> action = (object obj) =>
            {
                Console.WriteLine("Message : " + message.str);
                Console.WriteLine("[{0}]", string.Join(",", message.array));
            };
            
            Task t = new Task(action, "tcp");
            t.Start();

            return t;
            // throw new NotImplementedException();
        }
    }
}

// The framework exposes options for how to bind
public class TcpFrameworkOptions
{
    public IPEndPoint EndPoint { get; set; }
}

// The framework exposes a message parser used to parse incoming protocol messages from the network
public interface IFrameworkMessageParser
{
    bool TryParseMessage(ref ReadOnlySequence<byte> buffer, out Message message);
}

public class FrameworkMessageParser : IFrameworkMessageParser
{
    public bool TryParseMessage(ref ReadOnlySequence<byte> buffer, out Message message)
    {
        Console.WriteLine(buffer);
        message = new Message();
        message.str = EncodingExtensions.GetString(Encoding.UTF8, buffer);
        message.array = BuffersExtensions.ToArray<byte>(buffer);
        // TODO: Implement logic here
        // throw new NotImplementedException();
        return true;
    }
}

public class Message
{
    public string str = "";
    public byte[] array;
    // TODO: Add properties relevant to your message type here
}
