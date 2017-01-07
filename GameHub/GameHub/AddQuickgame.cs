using Android.App;
using Android.Views;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Widget;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using Android.Support.Design.Widget;
using Android.Content;
using Android.Support.V4.View;
using Android.Support.V4.App;
using System.Collections.Generic;
using Java.Lang;
using GameHub.Fragments;
using Android.Views.Animations;
using Android.Content.PM;
using System;
using Refractored.Controls;
using System.Threading.Tasks;

namespace GameHub
{
    [Activity(Label = "AddQuickgame", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AddQuickgame : AppCompatActivity
    {
        Button b1, b2;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddQuickgame);
            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            this.SetSupportActionBar(toolBar);
            this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            this.SupportActionBar.Title = GetString(Resource.String.AddQuickgame_Title);

            Spinner dropdown = FindViewById<Spinner>(Resource.Id.spinner1);
            string[] platform = new string[] { "Steam", "Origin", "Uplay", "Battle.net", "XBOX LIVE", "PSN", "Skype", "LOL", "Discord" };
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, platform);
            dropdown.Adapter = adapter;


            Spinner dropdown2 = FindViewById<Spinner>(Resource.Id.spinner2);
            string[] Duration = new string[] { " 1 ", " 2 ", " 3 ", " 5 ", " 10 ", " 24 " };
            ArrayAdapter<string> adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, Duration);
            dropdown2.Adapter = adapter2;

            var AutoComGameList = new string[] { "CS:GO", "", "LOL", "DOTA", "HS", "Watch Dogs 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "GTA 5", "Tomb Raider: Rise of the" };
            ArrayAdapter AutoComGameListAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, AutoComGameList);
            var TextGamelist = FindViewById<AutoCompleteTextView>(Resource.Id.autoCompleteTextView1);
            TextGamelist.Adapter = AutoComGameListAdapter;

            b1 = FindViewById<Button>(Resource.Id.button1);
            b2 = FindViewById<Button>(Resource.Id.button2);
            b2.Click += B2_Click;
            b1.Click += B1_Click;

            // Create your application here
        }

        public override void OnBackPressed()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent);
            Finish();
        }
        private void B2_Click(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            TimePickerDialog dialog = new TimePickerDialog(this, OnTimeSet, time.Hour, time.Minute, true);
            dialog.Show();
        }

        private void B1_Click(object sender, EventArgs e)
        {
            DateTime Today = DateTime.Today;
            DatePickerDialog dialog = new DatePickerDialog(this, OnDateSet, Today.Year, Today.Month - 1, Today.Day);
            dialog.DatePicker.MinDate = Today.Millisecond;
            dialog.Show();
        }
        void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            b1.Text = e.Date.ToLongDateString();
        }

        void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {

            b2.Text = Convert.ToString(e.HourOfDay) + ":" + Convert.ToString(e.Minute);
            b2.Text = string.Format("{0:00}:{1:00}", e.HourOfDay, e.Minute);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menut_save:
                    break;
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }

            //Toast.MakeText(this, "Zaznaczone: " + item.TitleFormatted,
            //    ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);

        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.AddQuickgameToolbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }
    }
}