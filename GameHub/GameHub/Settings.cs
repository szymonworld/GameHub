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
using Android.Text;
using Android.Text.Method;

namespace GameHub
{
    [Activity(Label = "Settings", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Settings : AppCompatActivity
    {
        private static string LoginDataUser;
        private static string Notification;
        private ISharedPreferences pref;
        private Switch sw;
        Intent intent;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings);

            pref = GetSharedPreferences(Notification, FileCreationMode.Private);
            RestoreData();


            sw = FindViewById<Switch>(Resource.Id.switch1);
            sw.Click += delegate
            {
                    SaveData();
            };


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
                var builder = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.DarkThemeDialog);
                builder.SetTitle("O GameHub");
                builder.SetMessage("Autorzy: \n\nSzymon Antonik \nSzymon Holisz \nS�awomir Wiktor\nB�a�ej �y�bicki ");
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
                var builder = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.DarkThemeDialog);
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

            Button reportbug = FindViewById<Button>(Resource.Id.reportbug);
            reportbug.Click += delegate
            {
                var builder = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.DarkThemeDialog);
                builder.SetTitle("Zg�o� B��d");
                TextView tv = new TextView(this);
                tv.TextFormatted = Html.FromHtml("<a href=\"mailto:error@gamehub.com\">error@gamehub.com</a>");
                tv.MovementMethod = LinkMovementMethod.Instance;
                tv.SetPadding(40,20,0,0);
                tv.SetTextSize(Android.Util.ComplexUnitType.Sp, 16);
                builder.SetView(tv);
                
                builder.SetPositiveButton("OK", (EventHandler<DialogClickEventArgs>)null);
                var dialog = builder.Create();
                dialog.Show();
                var okbutton = dialog.GetButton((int)DialogButtonType.Positive);
                okbutton.Click += delegate
                {
                    dialog.Cancel();
                };
            };
            Button tpn = FindViewById<Button>(Resource.Id.tpn);
            tpn.Click += delegate
            {
                var builder = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.DarkThemeDialog);
                builder.SetTitle("Wykorzystane materia�y:");
                builder.SetMessage("...");
                builder.SetPositiveButton("OK", (EventHandler<DialogClickEventArgs>)null);
                var dialog = builder.Create();
                dialog.Show();
                var okbutton = dialog.GetButton((int)DialogButtonType.Positive);
                okbutton.Click += delegate
                {
                    dialog.Cancel();
                };
            };
            Button priv = FindViewById<Button>(Resource.Id.priv);
            priv.Click += delegate
            {
                var builder = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.DarkThemeDialog);
                builder.SetTitle("Prywatno��");
                builder.SetMessage("To, co robisz oraz informacje, kt�rych dostarczasz \nGromadzimy tre�ci i inne informacje otrzymywane od Ciebie podczas korzystania z naszych us�ug, np.dane podawane przy rejestracji konta na Gamehub, tworzeniu tre�ci i ich udost�pnianiu oraz przy wymianie wiadomo�ci i innego rodzaju komunikat�w z innymi osobami.Mog� to by� informacje zawarte w dostarczanych przez Ciebie tre�ciach lub ich dotycz�ce, np.informacje o miejscu wykonania zdj�cia lub dacie utworzenia pliku.Gromadzimy te� informacje na temat sposob�w korzystania przez Ciebie z us�ug, np.rodzaju tre�ci, kt�re przegl�dasz i na kt�re reagujesz, a tak�e cz�stotliwo�ci i czasu trwania r�nych Twoich dzia�a�.");
                builder.SetPositiveButton("OK", (EventHandler<DialogClickEventArgs>)null);
                var dialog = builder.Create();
                dialog.Show();
                var okbutton = dialog.GetButton((int)DialogButtonType.Positive);
                okbutton.Click += delegate
                {
                    dialog.Cancel();
                };
            };
            Button site = FindViewById<Button>(Resource.Id.gamehubSite);
            site.Click += delegate
            {
                string Link = "http://2bdesign.lh.pl/all/gamehub/";
                Intent intent = new Intent(this, typeof(Browser));
                intent.PutExtra("BrowserLink", Link);
                OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                this.StartActivity(intent);
            };
        }


        private void SaveData()
        {
            ISharedPreferencesEditor PrefEdit = pref.Edit();
            if (sw.Checked)
            {
                PrefEdit.PutString("Notification", "yes");
            }
            else
            {
                PrefEdit.PutString("Notification", "no");
            }
            PrefEdit.Apply();
        }
        private void RestoreData()
        {
            string noti = "";
            sw = FindViewById<Switch>(Resource.Id.switch1);
            noti = pref.GetString("Notification", "");
            if (noti == "yes")
            {
                sw.Checked = true;
            }
            else
            {
                sw.Checked = false;
            }
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