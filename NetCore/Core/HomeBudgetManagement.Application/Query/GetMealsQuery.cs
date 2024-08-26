using HomeBudgetManagement.DTO;
using MediatR;

namespace HomeBudgetManagement.Application.Query
{
    public class GetMealsQuery : IRequest<IEnumerable<MealDto>>
    {
        public bool? IsActive { get; set; }
    }
}
