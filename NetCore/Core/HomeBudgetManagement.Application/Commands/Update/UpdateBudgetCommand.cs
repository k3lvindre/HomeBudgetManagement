using HomeBudgetManagement.SharedKernel.ValueObjects;
using MediatR;

namespace HomeBudgetManagement.Application.Commands
{
    public class UpdateBudgetCommand: IRequest<bool>
    {
        public required int Id { get; set; }
        public required string Description { get; set; }
        public required double Amount { get; set; }
        public ItemType ItemType { get; set; }
        public byte[]? File { get; set; }
        public string? FileExtension { get; set; }
    }
}
