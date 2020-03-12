using System;
using System.Threading.Tasks;
using Core.EventStore.Contracts;
using IntegrationEvents;
using MySqlQueryService.MySqlConfig;

namespace MySqlQueryService.Projectors
{
    public class CustomerCreatedEventProjector : IProjector<CustomerCreatedForMySql>
    {

        private MySqlDbContext _context;
        
        public CustomerCreatedEventProjector(MySqlDbContext context)
        {
            _context = context;
        }
        
        public async Task HandleAsync(CustomerCreatedForMySql integrationEvent)
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
