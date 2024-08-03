using System.Text.Json.Serialization;

namespace HomeBudgetManagement.DTO
{
    public class CreateMealResponseDto
    {
        [JsonPropertyName("isCreated")]
        public bool IsCreated { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}