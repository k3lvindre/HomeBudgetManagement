using MediatR;

namespace HomeBudgetManagement.Application.Commands
{
    public class UpdateMealCommand: IRequest<bool>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required double Price { get; set; }
        public byte[]? File { get; set; }
        public string? FileExtension { get; set; }
    }
}
