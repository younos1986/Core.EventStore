using System;

namespace MongoCommandService.Dtos
{
    public class CustomerDto
    {
        
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
