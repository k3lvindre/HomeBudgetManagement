//****USE FOR General Reposisotry pattern rather that indivudal Repository****//

using HomeBudgetManagement.Application;
using HomeBudgetManagement.Core.Domain;
using HomeBudgetManagement.Core.Domain.AccountAggregate;
using HomeBudgetManagement.Core.Domain.ExpenseAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Infrastructure.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HomeBudgetManagementDbContext _context;
        private IDbContextTransaction? _transaction = null;
        private readonly IGenericRepository<Expense> _expenses;
        private readonly IGenericRepository<Account> _account;
        private readonly IMediator _mediator;

        public UnitOfWork(HomeBudgetManagementDbContext context,
            IGenericRepository<Expense> expenses,
            IGenericRepository<Account> account,
            IMediator mediator)
        {
            _context = context;
            _expenses = expenses;
            _account = account;
            _mediator = mediator;
        }

        public IGenericRepository<Expense> Expenses
        {
            get
            {
                return _expenses;
            }
        }

        public IGenericRepository<Account> Accounts
        {
            get
            {
                return _account;
            }
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
                                   .SelectMany(e => e.DomainEvents)
                                   .ToList();

            var tasks = domainEvents.Select(domainEvent => _mediator.Publish(domainEvent));

            foreach (var domainEntity in domainEntities)
                domainEntity.ClearDomainEvents();


            await Task.WhenAll(tasks);

            return result;
        }
    }
}
