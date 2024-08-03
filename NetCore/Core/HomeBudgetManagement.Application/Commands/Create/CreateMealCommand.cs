using HomeBudgetManagement.DTO;
using HomeBudgetManagement.SharedKernel.ValueObjects;
using MediatR;

namespace HomeBudgetManagement.Application.Commands
{
    public class CreateMealCommand: IRequest<CreateMealResponseDto>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required double Price { get; set; }
        public byte[]? File { get; set; }
        public string? FileExtension { get; set; }
    }
}
