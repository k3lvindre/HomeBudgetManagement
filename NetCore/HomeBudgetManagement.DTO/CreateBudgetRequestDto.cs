using System.Runtime.Serialization;

namespace HomeBudgetManagement.DTO
{
    [DataContract]
    public class CreateBudgetRequestDto
    {
        [DataMember(Name = "description")]
        public required string Description { get; set; }
        [DataMember(Name = "amount")]
        public required double Amount { get; set; }
        [DataMember(Name = "type")]
        public required int Type { get; set; }
        [DataMember(Name = "file")]
        public byte[]? File { get; set; }
        [DataMember(Name = "fileExtension")]
        public string? FileExtension { get; set; }
    }
}
