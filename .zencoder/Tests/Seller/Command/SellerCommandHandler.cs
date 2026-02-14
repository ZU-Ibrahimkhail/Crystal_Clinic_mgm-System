using System.Threading.Tasks;

namespace Zencoder.Tests.Seller.Command
{
    public class SellerCommandHandler
    {
        public Task Handle(SellerCommand command)
        {
            return Task.CompletedTask;
        }
    }
}
