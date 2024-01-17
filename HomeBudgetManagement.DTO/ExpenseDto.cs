using System.Text.Json.Serialization;

namespace HomeBudgetManagement.DTO
{
    public class ExpenseDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }
}