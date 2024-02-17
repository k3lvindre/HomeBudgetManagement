using HomeBudgetManagement.Core.Domain.BudgetAggregate;
using HomeBudgetManagement.Core.Events;
using HomeBudgetManagement.DTO;
using MediatR;

namespace HomeBudgetManagement.Application.Commands
{
    public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, CreateExpenseResponseDto>
    {
        private readonly IBudgetRepository _budgetRepository;

        public CreateBudgetCommandHandler(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public async Task<CreateExpenseResponseDto> Handle(CreateBudgetCommand command, CancellationToken cancellationToken)
        {

            var itemToAdd = new Budget()
            {
                Description = command.Description,
                Amount = command.Amount,
                ItemType = command.ItemType,
                CreatedDate = DateTime.Now,
                FileAttachment = null
            };

            itemToAdd.AddDomainEvent(new ModifiedEvent<Budget>(itemToAdd));

            await _budgetRepository.AddAsync(itemToAdd);

            return new CreateExpenseResponseDto()
            {
                IsCreated = true,
                Id = itemToAdd.Id
            };
        }
    }
}
