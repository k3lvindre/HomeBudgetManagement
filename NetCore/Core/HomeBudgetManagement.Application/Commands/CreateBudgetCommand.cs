using HomeBudgetManagement.DTO;
using HomeBudgetManagement.SharedKernel.ValueObjects;
using MediatR;

namespace HomeBudgetManagement.Application.Commands
{
    public class CreateBudgetCommand: IRequest<CreateExpenseResponseDto>
    {
        public required string Description { get; set; }
        public required double Amount { get; set; }
        public ItemType ItemType { get; set; }
        public byte[]? File { get; set; }
        public string? FileExtension { get; set; }
        public CreateBudgetCommand()
        {
        }
    }
}
