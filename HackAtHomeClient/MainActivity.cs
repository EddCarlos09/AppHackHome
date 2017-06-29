using Android.App;
using Android.Widget;
using Android.OS;
using HackAtHome.SAL;
using HackAtHome.Entities;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/Icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);
            var BtnValidate = FindViewById<Button>(Resource.Id.buttonValidate);
            var UserMail = FindViewById<EditText>(Resource.Id.EditTextEmail);
            var UserPass = FindViewById<EditText>(Resource.Id.EditTextPassword);
            BtnValidate.Click += async(object sender, System.EventArgs e) =>
            {
                BtnValidate.Enabled = false;
                string StudentEmail = UserMail.Text;
                string StudentPassword = UserPass.Text;
                if (!string.IsNullOrEmpty(StudentEmail) && !string.IsNullOrEmpty(StudentPassword))
                {

                    ServiceClient ServiceClient = new ServiceClient();
                    ResultInfo Result = await ServiceClient.AutenticateAsync(StudentEmail, StudentPassword);
                    if (Result.Status == Status.Success)
                    {
                        var MicrosoftEvidence = new LabItem
                        {
                            Email = StudentEmail,
                            Lab = "Hack@Home",
                            DeviceId = Android.Provider.Settings.Secure.GetString(
                                ContentResolver, Android.Provider.Settings.Secure.AndroidId)
                        };
                        MicrosoftServiceClient MicrosoftService = new MicrosoftServiceClient();
                        await MicrosoftService.SendEvidence(MicrosoftEvidence);

                        var EvidenceIntent = new Android.Content.Intent(this, typeof(EvidenceActivity));
                        EvidenceIntent.PutExtra("Token", Result.Token);
                        EvidenceIntent.PutExtra("UserName", Result.FullName);
                        StartActivity(EvidenceIntent);
                        UserMail.Text = string.Empty;
                        UserPass.Text = string.Empty;
                    }
                    else
                    {
                        string ErrorMessageAuth = GetString(Resource.String.ErrorLogin);
                        var ErrorDialog = new AlertDialog.Builder(this);
                        ErrorDialog.SetMessage(ErrorMessageAuth);
                        ErrorDialog.SetNegativeButton(GetString(Resource.String.Ok), delegate { });
                        ErrorDialog.Show();
                    }
                    BtnValidate.Enabled = true;
                }             
            };
        }

        protected override void OnStop()
        {
            base.OnStop();

        }
    }
}

