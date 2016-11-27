using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using System.Threading.Tasks;
using Android.Util;
using Android.Content.PM;

namespace GameHub
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, Icon = "@drawable/icon4", NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            ActionBar.Hide();
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() => {
                Log.Debug(TAG, "Work is finished - start Activity1.");
                StartActivity(new Intent(Application.Context, typeof(LoginSystem)));
            });

            //startupWork.ContinueWith(t => {
            //    Log.Debug(TAG, "Work is finished - start Activity1.");
            //    StartActivity(new Intent(Application.Context, typeof(LoginSystem)));
            //}, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }
    }
}