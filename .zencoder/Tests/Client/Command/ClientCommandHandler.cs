using System.Threading.Tasks;

namespace Zencoder.Tests.Client.Command
{
    public class ClientCommandHandler
    {
        public Task Handle(ClientCommand command)
        {
            return Task.CompletedTask;
        }
    }
}
