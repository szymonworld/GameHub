using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Content.PM;

namespace GameHub
{
    [Activity(Label = "Settings", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Settings : AppCompatActivity
    {
        private static string LoginDataUser;
        private ISharedPreferences pref;
        Intent intent;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Settings);
            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolBar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.Title = GetString(Resource.String.Settings_Title);

            Button Logout = FindViewById<Button>(Resource.Id.logout);
            Logout.Click += delegate
            {
                pref = GetSharedPreferences(LoginDataUser, FileCreationMode.Private);
                ISharedPreferencesEditor PrefEdit = pref.Edit();
                PrefEdit.Clear();
                PrefEdit.Apply();
                intent = new Intent(this, typeof(LoginSystem));
                intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.ClearTask | ActivityFlags.NewTask);
                Finish();
                StartActivity(intent);
                
            };
            Button Editprofil = FindViewById<Button>(Resource.Id.profiledit);
            Editprofil.Click += delegate
            {
                Intent intent = new Intent(this, typeof(ProfileSettings));
                OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                this.StartActivity(intent);
            };
            Button About = FindViewById<Button>(Resource.Id.about);
            About.Click += delegate
            {
                var builder = new Android.Support.V7.App.AlertDialog.Builder(this);
                builder.SetTitle("O GameHub");
                builder.SetMessage("Autorzy: \n\nSzymon Antonik \nSzymon Holisz \nS³awomir Wiktor ");
                builder.SetPositiveButton("OK", (EventHandler<DialogClickEventArgs>)null);
                var dialog = builder.Create();
                dialog.Show();
                var okbutton = dialog.GetButton((int)DialogButtonType.Positive);
                okbutton.Click += delegate
                {
                    dialog.Cancel();
                };
            };


            Button changes = FindViewById<Button>(Resource.Id.changes);
            changes.Click += delegate
            {
                var builder = new Android.Support.V7.App.AlertDialog.Builder(this);
                builder.SetTitle("Zmiany w tej wersji:");
                builder.SetMessage("-Poprawa Layoutu \n-ulepszone opcje");
                builder.SetPositiveButton("OK", (EventHandler<DialogClickEventArgs>)null);
                var dialog = builder.Create();
                dialog.Show();
                var okbutton = dialog.GetButton((int)DialogButtonType.Positive);
                okbutton.Click += delegate
                {
                    dialog.Cancel();
                };
            };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}