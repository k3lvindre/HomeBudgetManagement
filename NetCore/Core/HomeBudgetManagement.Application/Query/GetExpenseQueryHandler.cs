//using HomeBudgetManagement.Application.EventFeed;
//using HomeBudgetManagement.Core.Domain.ExpenseAggregate;
//using HomeBudgetManagement.DTO;
//using MediatR;

//namespace HomeBudgetManagement.Application.Query
//{
//    public class GetExpenseQueryHandler : IRequestHandler<GetExpenseQuery, IEnumerable<ExpenseDto>>
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public GetExpenseQueryHandler(IUnitOfWork unitOfWork, IEventFeed eventFeed)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<IEnumerable<ExpenseDto>> Handle(GetExpenseQuery query, CancellationToken cancellationToken)
//        {
//            var result = await _unitOfWork.Expenses.GetAllAsync();
//            if(query.ExpenseIds.Any()) result = result.Where(expense => query.ExpenseIds.Contains(expense.Id)).ToList();

//            //We can use automapper here
//            return result.Select<Expense, ExpenseDto>(x =>
//            {
//                return new ExpenseDto()
//                {
//                    Amount = x.Amount,
//                    Description = x.Description,
//                    Date = x.CreatedDate,
//                    Id = x.Id
//                };
//            });
//        }
//    }
//}
