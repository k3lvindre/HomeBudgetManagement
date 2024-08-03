using System.Runtime.Serialization;

namespace HomeBudgetManagement.DTO
{
    [DataContract]
    public record UpdateMealRequestDto
    {
        [DataMember(Name = "id")]
        public required int Id { get; set; }
        [DataMember(Name = "Name")]
        public required string Name { get; set; }
        [DataMember(Name = "description")]
        public string? Description { get; set; }
        [DataMember(Name = "amount")]
        public required double Price { get; set; }
        [DataMember(Name = "file")]
        public byte[]? File { get; set; }
        [DataMember(Name = "fileExtension")]
        public string? FileExtension { get; set; }
    }
}
