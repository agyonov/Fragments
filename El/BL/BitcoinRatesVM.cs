using Db;
using El.BL.Bitcoin;
using El.BL.Bitcoin.Models;
using El.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;

namespace El.BL
{
    public class BitcoinRatesVM : RootVM
    {
        private const string BT_DOWNLOAD_DT = "BitcoinRatesVM.BT_DOWNLOAD_DT";
        private const string BT_DOWNLOAD_DATA = "BitcoinRatesVM.BT_DOWNLOAD_DATA";

        private readonly BitcoinClient _bitcoinClient;
        private readonly CacheRepository _Cache;
        private readonly ILogger _Logger;

        public BitcoinRatesVM(BloggingContext efc, BitcoinClient BitcoinClient,
                            CacheRepository Cache, ILogger<BitcoinRatesVM> Logger) : base(efc)
        {
            // Store
            _bitcoinClient = BitcoinClient;
            _Cache = Cache;
            _Logger = Logger;

            // Create commands
            DownloadRatesCommand = new AsyncRelayCommand((ct) => DownloadRates(ct));
        }


        private List<RateItem> rates = new();
        public List<RateItem> Rates {
            get {
                var cashedRates = _Cache.Get<List<RateItem>>(BT_DOWNLOAD_DATA);
                if (cashedRates != default) {
                    rates = cashedRates;
                }

                return rates;
            }
            set {
                // Set into cache
                _Cache.Add(BT_DOWNLOAD_DATA, value);

                SetProperty(ref rates, value);
            }
        }

        private DateTimeOffset dT = new();
        public DateTimeOffset DT {
            get {

                var cashedDt = _Cache.Get<DateTimeOffset>(BT_DOWNLOAD_DT);
                if (cashedDt != default) {
                    dT = cashedDt;
                }

                return dT;
            }
            set {
                // Set into cache
                _Cache.Add(BT_DOWNLOAD_DT, value);

                SetProperty(ref dT, value);
            }
        }

        public IAsyncRelayCommand DownloadRatesCommand { get; }


        private async Task DownloadRates(CancellationToken ct)
        {
            // Get from Internet
            BitcoinResult? dr = null;

            try {
                // Load that thing
                dr = await  _bitcoinClient.GetBitcoinRates(ct).ConfigureAwait(false);
            } catch (Exception err) {
                _Logger.LogWarning(err, "Error when trying to load Bitcoin rates");
            }

            // Create result
            var res = new List<RateItem>();

            // Check
            if (dr != null) {
                res.Add(dr.Bpi.EUR);
                res.Add(dr.Bpi.USD);
                res.Add(dr.Bpi.GBP);

                // Attach
                DT = dr.Time.UpdatedISO;
            } else {
                // Attach
                DT = DateTimeOffset.MinValue;
            }

            // Set rates
            Rates = res;
        }
    }
}
