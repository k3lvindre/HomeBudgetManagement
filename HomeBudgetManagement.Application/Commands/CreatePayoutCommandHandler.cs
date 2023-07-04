using HomeBudgetManagement.Core.Domain;
using HomeBudgetManagement.Core.Domain.ExpenseAggregate;
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
            //await _unitOfWork.Expenses.AddAsync(new Expense()
            //{
            //    Id = 13,
            //    AccountId = command.AccountId ?? 0,
            //    Amount = command.Amount,
            //    Description = command.Description,
            //    File = command.File,
            //    FileExtension = command.FileExtension,
            //    Type = command.Type,
            //    CreatedDate = DateTime.Now
            //});

            //int result = await _unitOfWork.SaveChangesAsync();

            return new CreateExpenseResponseDto()
            {
                IsCreated = 1 > 0
            };
        }
    }
}
