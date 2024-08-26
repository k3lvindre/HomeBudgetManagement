using HomeBudgetManagement.Core.Domain.MealAggregate;
using HomeBudgetManagement.DTO;
using HomeBudgetManagement.SharedKernel.ValueObjects;
using MediatR;

namespace HomeBudgetManagement.Application.Query
{
    public class GetBudgetQueryHandler(IMealRepository budgetRepository) : IRequestHandler<GetExpenseQuery, IEnumerable<MealDto>>
    {
        private readonly IMealRepository _budgetRepository = budgetRepository;

        public async Task<IEnumerable<MealDto>> Handle(GetExpenseQuery query, CancellationToken cancellationToken)
        {
            //Tech debt: get filtered items instead of getting all items
            //Tech debt: add automapper for dto
            //Tech debt: remove multiple calls of ToList
            var result = await _budgetRepository.GetAllAsync();

            if (query.ListOfId is not null)
            {
                if (query.ListOfId.Count != 0) result = result.Where(expense => query.ListOfId.Contains(expense.Id)).ToList();
                return result.ConvertAll<MealDto>(ConvertToDto);
            }

            //if (query.Type is not null)
            //{
            //    result = result.Where(expense => expense.ItemType == query.Type).ToList();
            //}

            if(!(string.IsNullOrEmpty(query.DateFrom) && string.IsNullOrEmpty(query.DateTo)))
            {
                var dateFrom = DateTime.ParseExact(query.DateFrom!, "yyyy-MM-dd", null);
                var dateTo = DateTime.ParseExact(query.DateTo!, "yyyy-MM-dd", null).AddHours(23).AddMinutes(59).AddSeconds(59);

                result = result.Where(x => x.CreatedDate >= dateFrom && x.CreatedDate <= dateTo).ToList();
            }

            return result.ConvertAll<MealDto>(ConvertToDto);
        }

        private MealDto ConvertToDto(Meal budget) => 
            new MealDto() 
            { 
                Id = budget.Id,
                Name = budget.Name,
                Description = budget.Description,
                Price = budget.Price, 
                CreatedDate = budget.CreatedDate, 
            };
    }
}
