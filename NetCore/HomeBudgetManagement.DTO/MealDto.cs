using System.Text.Json.Serialization;

namespace HomeBudgetManagement.DTO
{
    public record MealDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}