using System.Runtime.Serialization;

namespace HomeBudgetManagement.DTO
{
    public class GetBudgetQueryRequestDto
    {
        [DataMember(Name = "listOfId")]
        public List<int>? ListOfId { get; set; }

        [DataMember(Name = "type")]
        public int? Type { get; set; }

        [DataMember(Name = "dateFrom")]
        public string? DateFrom { get; set; }

        [DataMember(Name = "dateTo")]
        public string? DateTo { get; set; }
    }
}
