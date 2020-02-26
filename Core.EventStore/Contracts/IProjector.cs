using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventStore.Contracts
{
    public interface IProjector<IEvent> where IEvent : INotification
    {
        Task HandleAsync(IEvent integrationEvent);
    }
}
