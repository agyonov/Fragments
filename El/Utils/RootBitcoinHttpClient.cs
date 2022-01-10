using El.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace El.Utils
{
    public class RootBitcoinHttpClient : HttpClient
    {
        // Some DNS timeout
        private const double MinDnsSecodsToWait = 15.0;

        // Options
        private readonly IOptions<AppSettings> _appSettings;

        // Json Settings
        private readonly JsonSerializerOptions _jsonSettings;
        public JsonSerializerOptions JsonSettings => _jsonSettings;

        public RootBitcoinHttpClient(IOptions<AppSettings> AppSettings) : base(new SocketsHttpHandler
        {
            ConnectTimeout = TimeSpan.FromSeconds(MinDnsSecodsToWait),

        })
        {
            // Store
            _appSettings = AppSettings;

            // Set timeout and other sesstings
            Timeout = TimeSpan.FromSeconds(_appSettings.Value.ApiCmdTimeout);
            DefaultRequestVersion = new Version(2, 0);
            MaxResponseContentBufferSize = 134217728; // 128Mb
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
