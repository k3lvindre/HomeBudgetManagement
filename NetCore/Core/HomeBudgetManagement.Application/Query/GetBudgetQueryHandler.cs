using HomeBudgetManagement.Core.Domain.BudgetAggregate;
using HomeBudgetManagement.DTO;
using HomeBudgetManagement.SharedKernel.ValueObjects;
using MediatR;

namespace HomeBudgetManagement.Application.Query
{
    public class GetBudgetQueryHandler : IRequestHandler<GetExpenseQuery, IEnumerable<BudgetDto>>
    {
        private readonly IBudgetRepository _budgetRepository;

        public GetBudgetQueryHandler(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        public async Task<IEnumerable<BudgetDto>> Handle(GetExpenseQuery query, CancellationToken cancellationToken)
        {
            var result = await _budgetRepository.GetAllAsync();

            if (query.ListOfId is not null)
            {
                if (query.ListOfId.Any()) result = result.Where(expense => query.ListOfId.Contains(expense.Id)).ToList();
                return result.ConvertAll<BudgetDto>(ConvertToDto);
            }

            if (query.Type is not null)
            {
                result = result.Where(expense => expense.ItemType == query.Type).ToList();
                return result.ConvertAll<BudgetDto>(ConvertToDto);
            }

            return result.ConvertAll<BudgetDto>(ConvertToDto);
        }

        private BudgetDto ConvertToDto(Budget budget) => 
            new BudgetDto() 
            { 
                Amount = budget.Amount, 
                Description = budget.Description,
                Date = budget.CreatedDate,
                Id = budget.Id
            };
    }
}
