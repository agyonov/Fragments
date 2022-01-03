using System.Text.Json.Serialization;

namespace El.BL.Bitcoin.Models
{
    public record class RateItem
    {
        [JsonPropertyName("code")]
        public string Code { get; init; } = default!;
        [JsonPropertyName("symbol")]
        public string Symbol { get; init; } = default!;
        [JsonPropertyName("rate")]
        public string Rate { get; init; } = default!;
        [JsonPropertyName("description")]
        public string Description { get; init; } = default!;
        [JsonPropertyName("rate_float")]
        public double RateFloat { get; init; } = default!;
    }

    public record class Rates
    {
        [JsonPropertyName("USD")]
        public RateItem USD { get; init; } = default!;
        [JsonPropertyName("GBP")]
        public RateItem GBP { get; init; } = default!;
        [JsonPropertyName("EUR")]
        public RateItem EUR { get; init; } = default!;
    }

    public record TimeRecord(string updated, DateTimeOffset UpdatedISO);

    public record BitcoinResult(TimeRecord Time, Rates Bpi);
}
