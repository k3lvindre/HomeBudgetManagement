using HomeBudgetManagement.Core.Domain.BudgetAggregate;
using HomeBudgetManagement.Core.Events;
using MediatR;

namespace HomeBudgetManagement.Application.Commands
{
    public class DeleteBudgetCommandHandler(IBudgetRepository budgetRepository) : IRequestHandler<DeleteBudgetCommand, bool>
    {
        private readonly IBudgetRepository _budgetRepository = budgetRepository;

        public async Task<bool> Handle(DeleteBudgetCommand command, CancellationToken cancellationToken)
        {
            var itemToDelete = new Budget()
            {
                Id = command.Id,
            };

            itemToDelete.AddDomainEvent(new ModifiedEvent<Budget>(itemToDelete));
             _budgetRepository.Delete(itemToDelete);

            return await Task.FromResult(true);
        }
    }
}
