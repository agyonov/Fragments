using Db;
using El.BL.Bitcoin;
using El.BL.Bitcoin.Models;

namespace El.BL
{
    public class BitcoinRatesVM : RootVM
    {
        private readonly BitcoinClient _bitcoinClient;

        public BitcoinRatesVM(BloggingContext efc, BitcoinClient BitcoinClient) : base(efc)
        {
            _bitcoinClient = BitcoinClient;
        }

        public async Task<BitcoinResult?> test(CancellationToken ct)
        {
            var ha = await _bitcoinClient.GetBitcoinRates(ct).ConfigureAwait(false);
            return ha;
        }
    }
}
