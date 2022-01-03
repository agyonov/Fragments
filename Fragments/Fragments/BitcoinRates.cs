using El.BL;

namespace Fragments.Fragments
{
    public class BitcoinRates : GcFragment<BitcoinRatesVM>
    {
        public override void OnStart()
        {
            // call parent
            base.OnStart();

            _ = Task.Run(async () => await VM.test(CancellationToken.None));
        }
    }
}
