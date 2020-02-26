using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationEvents
{
    public class CustomerCreated : INotification
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }

        public CustomerCreated(string firstName, string lastName, DateTime createdOn)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CreatedOn = createdOn;
        }
    }
}
