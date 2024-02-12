//****USE FOR General Reposisotry pattern rather that indivudal Repository****//

using HomeBudgetManagement.Core.Domain;
using HomeBudgetManagement.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;

namespace HomeBudgetManagement.Infrastructure.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HomeBudgetManagementDbContext _context;
        private IDbContextTransaction? _transaction = null;
        private readonly IMediator _mediator;

        public UnitOfWork(HomeBudgetManagementDbContext context,
            IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public void CreateTransaction()
        {
            _transaction = _context.Database.CurrentTransaction ?? _context.Database.BeginTransaction();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await _context.SaveChangesAsync();

            var domainEntities = _context.ChangeTracker
                                    .Entries<BaseEntity>()
                                    .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                                    .Select(x=>x.Entity)
                                    .ToList();


            var domainEvents = domainEntities
                                   .SelectMany(e => e.DomainEvents!)
                                   .ToList();

            var tasks = domainEvents.Select(domainEvent => _mediator.Publish(domainEvent));

            foreach (var domainEntity in domainEntities)
                domainEntity.ClearDomainEvents();


            await Task.WhenAll(tasks);

            return result;
        }
    }
}
