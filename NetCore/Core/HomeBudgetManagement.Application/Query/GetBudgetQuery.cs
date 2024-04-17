using HomeBudgetManagement.DTO;
using HomeBudgetManagement.SharedKernel.ValueObjects;
using MediatR;

namespace HomeBudgetManagement.Application.Query
{
    public class GetExpenseQuery : IRequest<IEnumerable<BudgetDto>>
    {
        public List<int>? ListOfId { get; set; }
        public ItemType? Type { get; set; }
        public string? DateFrom { get; set; }
        public string? DateTo { get; set; }
    }
}
