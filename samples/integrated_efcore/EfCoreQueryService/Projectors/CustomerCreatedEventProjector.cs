using System;
using Core.EventStore.Contracts;
using IntegrationEvents;
using System.Threading.Tasks;
using EfCoreQueryService.EfCoreConfig;

namespace EfCoreQueryService.Projectors
{
    public class CustomerCreatedEventProjector : IProjector<CustomerCreatedForEfCore>
    {
        
        private EfCoreDbContext _context;
        
        public CustomerCreatedEventProjector(EfCoreDbContext context)
        {
            _context = context;
        }
        
        public async Task HandleAsync(CustomerCreatedForEfCore integrationEvent)
        {
            try
            {
                await _context.Customers.AddAsync(integrationEvent);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
