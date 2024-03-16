using HomeBudgetManagement.Core.Domain.BudgetAggregate;
using HomeBudgetManagement.Core.Events;
using MediatR;

namespace HomeBudgetManagement.Application.Commands
{
    public class UpdateBudgetCommandHandler(IBudgetRepository budgetRepository) : IRequestHandler<UpdateBudgetCommand, bool>
    {
        private readonly IBudgetRepository _budgetRepository = budgetRepository;

        public async Task<bool> Handle(UpdateBudgetCommand command, CancellationToken cancellationToken)
        {
            var itemToUpdate = new Budget()
            {
                Id = command.Id,
                Description = command.Description,
                Amount = command.Amount,
                ItemType = command.ItemType,
                FileAttachment = null
            };

            itemToUpdate.AddDomainEvent(new ModifiedEvent<Budget>(itemToUpdate));
             _budgetRepository.Update(itemToUpdate);

            return await Task.FromResult(true);
        }
    }
}
