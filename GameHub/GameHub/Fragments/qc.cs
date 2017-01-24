using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Widget;
using Android.Support.V7.App;

namespace GameHub.Fragments
{
    public class qc : SupportFragment
    {
        Button b1, b2;
        View view;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            view = inflater.Inflate(Resource.Layout.AddQuickgame, container, false);

            SupportToolbar toolBar = view.FindViewById<SupportToolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)this.Activity).SetSupportActionBar(toolBar);
            ((AppCompatActivity)this.Activity).SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            ((AppCompatActivity)this.Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            ((AppCompatActivity)this.Activity).SupportActionBar.Title = GetString(Resource.String.AddQuickgame_Title);


            var AutoComGameList = new String[] { "CS:GO", "", "LOL", "DOTA", "HS", "Watch Dogs 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "GTA 5", "Tomb Raider: Rise of the" };
            ArrayAdapter AutoComGameListAdapter = new ArrayAdapter(((AppCompatActivity)this.Activity), Android.Resource.Layout.SimpleDropDownItem1Line, AutoComGameList);
            var TextGamelist = view.FindViewById<AutoCompleteTextView>(Resource.Id.autoCompleteTextView1);
            TextGamelist.Adapter = AutoComGameListAdapter;

            b1 = view.FindViewById<Button>(Resource.Id.date);
            b2 = view.FindViewById<Button>(Resource.Id.hours);
            b2.Click += B2_Click;
            b1.Click += B1_Click;
            return view;
        }

        private void B2_Click(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            TimePickerDialog dialog = new TimePickerDialog(((AppCompatActivity)this.Activity), OnTimeSet, time.Hour, time.Minute, true);
            dialog.Show();
        }

        private void B1_Click(object sender, EventArgs e)
        {
            DateTime Today = DateTime.Today;
            DatePickerDialog dialog = new DatePickerDialog(((AppCompatActivity)this.Activity), OnDateSet, Today.Year, Today.Month - 1, Today.Day);
            dialog.DatePicker.MinDate = Today.Millisecond;
            dialog.Show();
        }
        void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            b1.Text = e.Date.ToLongDateString();
        }

        void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            
            b2.Text = Convert.ToString(e.HourOfDay) +":" + Convert.ToString(e.Minute);
            b2.Text = string.Format("{0:00}:{1:00}", e.HourOfDay, e.Minute);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menut_save:
                    break;
                case Android.Resource.Id.Home:
                    ((AppCompatActivity)this.Activity).Finish();
                    return true;
            }

            //Toast.MakeText(this, "Zaznaczone: " + item.TitleFormatted,
            //    ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);

        }
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.AddQuickgameToolbar, menu);
        }


    }
}