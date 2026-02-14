using System.Threading.Tasks;

namespace Zencoder.Tests.Product.Command
{
    public class ProductCommandHandler
    {
        public Task Handle(ProductCommand command)
        {
            return Task.CompletedTask;
        }
    }
}
