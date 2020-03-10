using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationEvents
{
    public class CustomerCreatedForMySql : INotification
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }

        public CustomerCreatedForMySql(Guid id,string firstName, string lastName, DateTime createdOn)
        {
            this.Id = id; 
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CreatedOn = createdOn;
        }
    }
}
