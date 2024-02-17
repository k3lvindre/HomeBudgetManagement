﻿using HomeBudgetManagement.Core.ValueObject;
using MediatR;
using System.Text.Json.Serialization;

namespace HomeBudgetManagement.Core.Domain
{
    public abstract record BaseEntity(
        int Id, 
        string Description,
        double Amount,
        ItemType ItemType,
        DateTime CreatedDate) 
    {

        [JsonIgnore]
        private List<INotification>? _domainEvents;

        [JsonIgnore]
        public IReadOnlyCollection<INotification>? DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem) => _domainEvents?.Remove(eventItem);

        public void ClearDomainEvents() => _domainEvents?.Clear();
    }
}