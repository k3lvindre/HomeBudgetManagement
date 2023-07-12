using HomeBudgetManagement.Core.Events;
using MediatR;

namespace HomeBudgetManagement.Application.DomainEventHandlers
{
    //you can also use event bus here to publish events for mircorservice in case you have one.
    //that is called integration events
    //but the example here is called domain events
    public class ExpenseModifiedEventHandler : INotificationHandler<ExpenseModifiedEvent>
    {
        IUnitOfWork _unitOfWork;

        public ExpenseModifiedEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ExpenseModifiedEvent notification, CancellationToken cancellationToken)
        {
            var expense = notification.Expense;
            var account = await _unitOfWork.Accounts.GetByIdAsync(expense.AccountId);
            if (account is not null)
            {
                account.Balance -= expense.Amount;
                _unitOfWork.Accounts.Update(account);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
