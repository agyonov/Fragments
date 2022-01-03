using Db;
using El.BL.Bitcoin;
using El.BL.Bitcoin.Models;
using Microsoft.Toolkit.Mvvm.Input;

namespace El.BL
{
    public class BitcoinRatesVM : RootVM
    {
        private readonly BitcoinClient _bitcoinClient;

        public BitcoinRatesVM(BloggingContext efc, BitcoinClient BitcoinClient) : base(efc)
        {
            // Store
            _bitcoinClient = BitcoinClient;

            // Create commands
            DownloadRatesCommand = new AsyncRelayCommand((ct) => DownloadRates(ct));
        }


        private List<RateItem> rates = new List<RateItem>();
        public List<RateItem> Rates
        {
            get => rates;
            set => SetProperty(ref rates, value);
        }

        private DateTimeOffset dT = new DateTimeOffset();
        public DateTimeOffset DT
        {
            get => dT;
            set => SetProperty(ref dT, value);
        }

        public IAsyncRelayCommand DownloadRatesCommand { get; }


        private async Task DownloadRates(CancellationToken ct)
        {
            // Get from Internet
            var dr = await _bitcoinClient.GetBitcoinRates(ct).ConfigureAwait(false);

            // Create result
            var res = new List<RateItem>();

            // Check
            if (dr != null) {
                res.Add(dr.Bpi.EUR);
                res.Add(dr.Bpi.USD);
                res.Add(dr.Bpi.GBP);

                // Attach
                DT = dr.Time.UpdatedISO;
            }

            // Attach
            Rates = res;
        }
    }
}
