using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;

namespace NationalPlatform.Models;


public class TcpConnectionHandlers : ConnectionHandler
{
    private readonly ILogger<TcpConnectionHandlers> _logger;

    public TcpConnectionHandlers(ILogger<TcpConnectionHandlers> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        _logger.LogInformation(connection.ConnectionId + " connected");

        Console.WriteLine("connected");

        while (true)
        {
            var result = await connection.Transport.Input.ReadAsync();
            var buffer = result.Buffer;
            
            foreach (var segment in buffer)
            {
                await connection.Transport.Output.WriteAsync(segment);
            }

            if (result.IsCompleted)
            {
                break;
            }

            connection.Transport.Input.AdvanceTo(buffer.End);
        }

        _logger.LogInformation(connection.ConnectionId + " disconnected");
    }
}
