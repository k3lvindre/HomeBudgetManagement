using HomeBudgetManagement.Core.Domain.MealAggregate;
using HomeBudgetManagement.Core.Events;
using MediatR;

namespace HomeBudgetManagement.Application.Commands
{
    public class DeleteMealCommandHandler(IMealRepository budgetRepository) : IRequestHandler<DeleteMealCommand, bool>
    {
        private readonly IMealRepository _budgetRepository = budgetRepository;

        public async Task<bool> Handle(DeleteMealCommand command, CancellationToken cancellationToken)
        {
            var itemToDelete = new Meal()
            {
                Id = command.Id,
            };

            itemToDelete.AddDomainEvent(new ModifiedEvent<Meal>(itemToDelete));
             _budgetRepository.Delete(itemToDelete);

            return await Task.FromResult(true);
        }
    }
}
