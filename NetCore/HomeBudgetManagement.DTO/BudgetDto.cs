using System.Text.Json.Serialization;

namespace HomeBudgetManagement.DTO
{
    public record BudgetDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } 

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }
}