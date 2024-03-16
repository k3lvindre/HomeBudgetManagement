using System.Runtime.Serialization;

namespace HomeBudgetManagement.DTO
{
    [DataContract]
    public record DeleteBudgetRequestDto
    {
        [DataMember(Name = "id")]
        public required int Id { get; set; }
    }
}
