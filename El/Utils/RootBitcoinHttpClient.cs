using El.Models;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace El.Utils
{
    public class RootBitcoinHttpClient : HttpClient
    {
        // Options
        private readonly IOptions<AppSettings> _appSettings;

        // Json Settings
        private readonly JsonSerializerOptions _jsonSettings;
        public JsonSerializerOptions JsonSettings => _jsonSettings;

        public RootBitcoinHttpClient(IOptions<AppSettings> AppSettings, HttpMessageHandler handler) : base(handler, true)
        {
            // Store
            _appSettings = AppSettings;

            // Set timeout and other sesstings
            Timeout = TimeSpan.FromSeconds(_appSettings.Value.ApiCmdTimeout);
            DefaultRequestVersion = new Version(2, 0);
            MaxResponseContentBufferSize = 134217728; // 128Mb
            DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
            BaseAddress = new Uri("https://api.coindesk.com/v1/");

            // Set JSON serializer
            _jsonSettings = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReadCommentHandling = JsonCommentHandling.Skip,
                WriteIndented = false,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };
        }
    }
}
