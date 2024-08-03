using HomeBudgetManagement.Core.Domain.MealAggregate;
using HomeBudgetManagement.Core.Events;
using MediatR;

namespace HomeBudgetManagement.Application.Commands
{
    public class UpdateMealCommandHandler(IMealRepository budgetRepository) : IRequestHandler<UpdateMealCommand, bool>
    {
        private readonly IMealRepository _budgetRepository = budgetRepository;

        public async Task<bool> Handle(UpdateMealCommand command, CancellationToken cancellationToken)
        {
            var itemToUpdate = new Meal()
            {
                Id = command.Id,
                Description = command.Description,
                Price = command.Price,
                FileAttachment = null
            };

            itemToUpdate.AddDomainEvent(new ModifiedEvent<Meal>(itemToUpdate));
             _budgetRepository.Update(itemToUpdate);

            return await Task.FromResult(true);
        }
    }
}
