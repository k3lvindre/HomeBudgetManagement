using HomeBudgetManagement.Core.Domain.ExpenseAggregate;
using HomeBudgetManagement.Core.Events;
using HomeBudgetManagement.Core.ValueObject;
using HomeBudgetManagement.DTO;
using MediatR;

namespace HomeBudgetManagement.Application.Commands
{
    public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, CreateExpenseResponseDto>
    {
        private readonly IExpenseRepository _expenseRepository;

        public CreateExpenseCommandHandler(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<CreateExpenseResponseDto> Handle(CreateExpenseCommand command, CancellationToken cancellationToken)
        {
            var Id = new Random().Next(0, 100);

            var expense = new Expense(
                Id,
                DateTime.Now,
                command.Description,
                ItemType.Expense,
                command.Amount,
                command.File,
                command.FileExtension);

            expense.AddDomainEvent(new ModifiedEvent<Expense>(expense));

            await _expenseRepository.AddAsync(expense);

            return new CreateExpenseResponseDto()
            {
                IsCreated = true,
                Id = expense.Id
            };
        }
    }
}
