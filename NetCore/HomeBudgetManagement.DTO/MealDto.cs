using System.Text.Json.Serialization;

namespace HomeBudgetManagement.DTO
{
    public record MealDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("createddate")]
        public DateTime CreatedDate { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}