using El.BL.Bitcoin.Models;
using El.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace El.BL.Bitcoin
{
    public class BitcoinClient : ClassRoot
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _settings;
        private readonly ILogger _Logger;

        private const double MinDnsSecodsToWait = 15.0;

        protected JsonSerializerOptions JsonSerializerSettings => _settings;

        #region Constructor and Destructor

        /// <summary>
        /// Initializing constructor
        /// </summary>
        /// <param name="AppSettings"></param>
        public BitcoinClient(IOptions<AppSettings> AppSettings, ILogger<BitcoinClient> Logger)
        {
            // Store
            _appSettings = AppSettings;
            _Logger = Logger;

            // Create http client
            _httpClient = new HttpClient(new SocketsHttpHandler
            {
                ConnectTimeout = TimeSpan.FromSeconds(MinDnsSecodsToWait)
            })
            {
                Timeout = TimeSpan.FromSeconds(_appSettings.Value.ApiCmdTimeout)
            };

            // Create JSON Settings
            _settings = CreateSerializerSettings();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        protected override void Dispose(bool flag)
        {
            // Free 
            if (flag) {
                // Dispose http Client
                if (_httpClient != null) {
                    _httpClient.Dispose();
                }
            }
        }

        protected override ValueTask DisposeAsync(bool flag)
        {
            // Free 
            if (flag) {
                // Dispose http Client
                if (_httpClient != null) {
                    _httpClient.Dispose();
                }
            }


            return ValueTask.CompletedTask;
        }

        #endregion Constructor and Destructor


        public async Task<BitcoinResult?> GetBitcoinRates(CancellationToken cancellationToken)
        {
            // set url
            var url = "https://api.coindesk.com/v1/bpi/currentprice.json";

            // Create reuquest object
            using (var request = new HttpRequestMessage()) {
                // Send it
                var response = await _httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);

                // Check
                if (response != null) {
                    // Get status
                    var status = (int)response.StatusCode;

                    // process status
                    if (status == 200) {
                        return await response.Content.ReadFromJsonAsync<BitcoinResult>(JsonSerializerSettings, cancellationToken).ConfigureAwait(false);
                    } else {
                        // Throw
                        throw new Exception("Http Api error");
                    }
                } else {
                    // return
                    return await Task.FromResult<BitcoinResult?>(null).ConfigureAwait(false);
                }
            }
        }

        private JsonSerializerOptions CreateSerializerSettings()
        {
            return new JsonSerializerOptions
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
