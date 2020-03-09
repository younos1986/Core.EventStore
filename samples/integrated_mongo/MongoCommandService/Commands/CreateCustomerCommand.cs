using MediatR;
using MongoCommandService.Dtos;

namespace MongoCommandService.Commands
{
    public class CreateCustomerCommand : IRequest<CustomerDto>
    {
        //public CreateCustomerCommand(string firstName, string lastName)
        //{
        //    FirstName = firstName;
        //    LastName = lastName;
        //}

        
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
