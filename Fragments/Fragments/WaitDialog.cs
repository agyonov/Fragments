using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fragments.Fragments
{
    public class WaitDialog : AndroidX.Fragment.App.DialogFragment
    {
        public override Dialog? OnCreateDialog(Bundle savedInstanceState)
        {
            // Use the Builder class for convenient dialog construction
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            builder.SetMessage("Wait, please...");
            builder.SetCancelable(false);

            // Create the AlertDialog object and return it
            return builder.Create();
        }
    }
}
