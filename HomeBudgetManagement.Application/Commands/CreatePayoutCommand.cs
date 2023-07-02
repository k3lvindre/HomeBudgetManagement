using HomeBudgetManagement.DTO;
using MediatR;

namespace HomeBudgetManagement.Application.Commands
{
    public class CreateExpenseCommand: IRequest<CreateExpenseResponseDto>
    {
        public required string Description { get; set; }
        public required string Type { get; set; }
        public required double Amount { get; set; }
        public byte[]? File { get; set; }
        public string? FileExtension { get; set; }
        public int? AccountId { get; set; }
        public CreateExpenseCommand()
        {
        }
    }
}
