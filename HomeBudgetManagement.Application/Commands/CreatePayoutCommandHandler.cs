using HomeBudgetManagement.Core.Domain.ExpenseAggregate;
using HomeBudgetManagement.Core.Events;
using HomeBudgetManagement.DTO;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Application.Commands
{
    public class CreatePayoutCommandHandler : IRequestHandler<CreateExpenseCommand, CreateExpenseResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePayoutCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateExpenseResponseDto> Handle(CreateExpenseCommand command, CancellationToken cancellationToken)
        {
            var expense = new Expense()
            {
                Id = new Random().Next(0, 100),
                AccountId = command.AccountId ?? 0,
                Amount = command.Amount,
                Description = command.Description,
                File = command.File,
                FileExtension = command.FileExtension,
                Type = command.Type,
                CreatedDate = DateTime.Now
            };

            expense.AddDomainEvent(new ExpenseModifiedEvent(expense));

            await _unitOfWork.Expenses.AddAsync(expense);

            //int result = await _unitOfWork.SaveChangesAsync();

            return new CreateExpenseResponseDto()
            {
                IsCreated = 1 > 0
            };
        }
    }
}
