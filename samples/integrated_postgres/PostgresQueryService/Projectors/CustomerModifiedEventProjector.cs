using System.Threading.Tasks;
using Core.EventStore.Contracts;
using IntegrationEvents;

namespace PostgresQueryService.Projectors
{
    public class CustomerModifiedEventProjector : IProjector<CustomerModified>
    {
        public async Task HandleAsync(CustomerModified integrationEvent)
        {
            await Task.CompletedTask;
        }
    }
}
