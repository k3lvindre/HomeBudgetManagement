using MediatR;

namespace HomeBudgetManagement.Application.Commands
{
    public class DeleteMealCommand: IRequest<bool>
    {
        public required int Id { get; set; }
    }
}
