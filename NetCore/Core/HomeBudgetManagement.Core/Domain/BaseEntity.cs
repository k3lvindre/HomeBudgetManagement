using MediatR;
using System.Text.Json.Serialization;

namespace HomeBudgetManagement.Core.Domain
{
    public abstract class BaseEntity
    {
        public  int Id { get; set; } = 0;
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        private List<INotification> _domainEvents;

        [JsonIgnore]
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
