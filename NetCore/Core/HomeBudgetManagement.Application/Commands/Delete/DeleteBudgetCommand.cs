using MediatR;

namespace HomeBudgetManagement.Application.Commands
{
    public class DeleteBudgetCommand: IRequest<bool>
    {
        public required int Id { get; set; }
    }
}
