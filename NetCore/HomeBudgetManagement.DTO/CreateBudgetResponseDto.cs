using System.Text.Json.Serialization;

namespace HomeBudgetManagement.DTO
{
    public class CreateBudgetResponseDto
    {
        [JsonPropertyName("isCreated")]
        public bool IsCreated { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}