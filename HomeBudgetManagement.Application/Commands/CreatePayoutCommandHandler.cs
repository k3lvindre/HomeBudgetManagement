using HomeBudgetManagement.Application.EventFeed;
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
        private readonly IEventFeed _eventFeed;

        public CreatePayoutCommandHandler(IUnitOfWork unitOfWork, IEventFeed eventFeed)
        {
            _unitOfWork = unitOfWork;
            _eventFeed = eventFeed;
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

            return new CreateExpenseResponseDto()
            {
                IsCreated = true
            };
        }
    }
}
