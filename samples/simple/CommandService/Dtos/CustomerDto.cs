using System;

namespace CommandService.Dtos
{
    public class CustomerDto
    {
        
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CreatedOn { get; set; }
    }
}
