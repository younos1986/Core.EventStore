using EfCoreCommandService.Dtos;
using MediatR;

namespace EfCoreCommandService.Commands
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
