using System.Runtime.Serialization;

namespace HomeBudgetManagement.Application.Commands
{
    [DataContract]
    public class CreateExpenseRequestDto
    {
        [DataMember(Name = "description")]
        public required string Description { get; set; }
        [DataMember(Name = "type")]
        public required string Type { get; set; }
        [DataMember(Name = "amount")]
        public required double Amount { get; set; }
        [DataMember(Name = "file")]
        public byte[]? File { get; set; }
        [DataMember(Name = "fileExtension")]
        public string? FileExtension { get; set; }
        [DataMember(Name = "accountId")]
        public int? AccountId { get; set; }
    }
}
