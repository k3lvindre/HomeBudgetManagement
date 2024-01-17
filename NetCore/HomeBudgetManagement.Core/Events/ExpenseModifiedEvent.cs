using HomeBudgetManagement.Core.Domain.ExpenseAggregate;
using MediatR;

namespace HomeBudgetManagement.Core.Events
{
    public class ExpenseModifiedEvent : INotification
    {
        public Expense Expense { get; }

        public ExpenseModifiedEvent(Expense expense)
        {
            Expense = expense;
        }
    }
}
