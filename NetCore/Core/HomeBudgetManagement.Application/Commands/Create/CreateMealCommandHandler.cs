using HomeBudgetManagement.Core.Domain.MealAggregate;
using HomeBudgetManagement.Core.Events;
using HomeBudgetManagement.DTO;
using MediatR;

namespace HomeBudgetManagement.Application.Commands
{
    public class CreateMealCommandHandler(IMealRepository budgetRepository) : IRequestHandler<CreateMealCommand, CreateMealResponseDto>
    {
        private readonly IMealRepository _budgetRepository = budgetRepository;

        public async Task<CreateMealResponseDto> Handle(CreateMealCommand command, CancellationToken cancellationToken)
        {

            var itemToAdd = new Meal()
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                CreatedDate = DateTime.Now,
                FileAttachment = null
            };

            itemToAdd.AddDomainEvent(new ModifiedEvent<Meal>(itemToAdd));
            await _budgetRepository.AddAsync(itemToAdd);

            return new CreateMealResponseDto()
            {
                IsCreated = true,
                Id = itemToAdd.Id
            };
        }
    }
}
