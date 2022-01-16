using El.BL.Bitcoin.Models;
using El.Interfaces;
using El.Utils;
using System.Net.Http.Json;

namespace El.BL.Bitcoin
{
    public class BitcoinClient : ClassRoot
    {
        // Locals
        private readonly RootBitcoinHttpClient _httpClient;
        private readonly IRetryService _Ret;

        #region Constructor and Destructor

        /// <summary>
        /// Initializing constructor
        /// </summary>
        /// <param name="AppSettings"></param>
        public BitcoinClient(RootBitcoinHttpClient HttpClient, IRetryService Ret)
        {
            // Store
            _httpClient = HttpClient;
            _Ret = Ret;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        protected override void Dispose(bool flag)
        {
            // Free 
            if (flag) {

            }
        }

        protected override ValueTask DisposeAsync(bool flag)
        {
            // Free 
            if (flag) {

            }


            return ValueTask.CompletedTask;
        }

        #endregion Constructor and Destructor

        // Get some rates
        public async Task<BitcoinResult?> GetBitcoinRates(CancellationToken cancellationToken)
        {
            // set url
            var url = "bpi/currentprice.json";

            // Send it
            var response = await _Ret.ExecuteWithParamRetry((lUrl, lCt) => _httpClient.GetAsync(lUrl, lCt), url, cancellationToken).ConfigureAwait(false);

            // Check
            if (response != null) {
                // Get status
                var status = (int)response.StatusCode;

                // process status
                if (status == 200) {
                    return await response.Content.ReadFromJsonAsync<BitcoinResult>(_httpClient.JsonSettings, cancellationToken).ConfigureAwait(false);
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
}
