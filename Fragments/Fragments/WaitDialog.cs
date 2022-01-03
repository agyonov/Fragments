namespace Fragments.Fragments
{
    public class WaitDialog : AndroidX.Fragment.App.DialogFragment
    {
        public override Dialog? OnCreateDialog(Bundle savedInstanceState)
        {
            // Use the Builder class for convenient dialog construction
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            builder.SetMessage(Resource.String.please_wait_text);
            builder.SetCancelable(false);


            // Create the AlertDialog object and return it
            return builder.Create();
        }
    }
}
