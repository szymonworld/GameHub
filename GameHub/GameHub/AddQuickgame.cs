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
using Android.Graphics;
using System.Json;
using System.Linq;
using Android.Views.InputMethods;

namespace GameHub
{
    [Activity(Label = "AddQuickgame", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AddQuickgame : AppCompatActivity
    {
        Button b1, b2;
        View view;
        private string LoginDataUser;
        private ISharedPreferences pref;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            pref = this.GetSharedPreferences(LoginDataUser, FileCreationMode.Private);
            SetContentView(Resource.Layout.AddQuickgame);
            view = FindViewById(Android.Resource.Id.Content);
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

            var AutoComGameList = new string[] { "CS:GO", "", "LOL", "DOTA", "Watch Dogs 2", "Call of Duty Modern Warfare 2", "GTA 5", "Tomb Raider: Rise of the", "Uncharted 4", "Dota 2", "Overwatch", "HearthStone", "World of Warcraft", "RANDOM", "LOSOWA GRA", "MineCraft", "StarCraft 2", "World of Tanks", "Heroes of the storm", "The Division", "Smite", "FIFA 17", "FIFA 16", "FIFA 15", "FIFA 14", "Poker", "ARK", "Black Desert Online", "Arma 3", "Day Z", "H1Z1", "Diablo 3", "Rainbow SIX: SIEGE", "DEstiny", "Rocket League", "Tibia", "Lineage 2", "Rust", "Gwint", "Battlefield 1" };
            ArrayAdapter AutoComGameListAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, AutoComGameList);
            var TextGamelist = FindViewById<AutoCompleteTextView>(Resource.Id.autoCompleteTextView1);
            TextGamelist.Adapter = AutoComGameListAdapter;

            b1 = FindViewById<Button>(Resource.Id.date);
            b2 = FindViewById<Button>(Resource.Id.hours);
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
                    AddQuickGame();
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

        private async void AddQuickGame()
        {
            CloseKey();
            bool internetConnection = await API.checkForInternetConnection();
            ProgressDialog dialog;
            if (internetConnection)
            {
                EditText gametitle = view.FindViewById<EditText>(Resource.Id.gametitle);
                dialog = new ProgressDialog(this, Resource.Style.AppCompatAlertDialogStyle);
                dialog.SetMessage(GetString(Resource.String.AddQuickgame_Adding));
                dialog.Show();
                if (gametitle.Text.Count() > 0)
                {
                    EditText title = view.FindViewById<EditText>(Resource.Id.gametitle);
                    if (title.Text.Count() > 0)
                    {
                        Button date = FindViewById<Button>(Resource.Id.date);
                        if (date.Text != "Data")
                        {
                            Button hours = FindViewById<Button>(Resource.Id.hours);
                            if (hours.Text != "Godzina")
                            {
                                Event ev = new Event();

                                Spinner dropdown = FindViewById<Spinner>(Resource.Id.spinner1);
                                Spinner dropdown2 = FindViewById<Spinner>(Resource.Id.spinner2);

                                bool eventCreated = await API.createEvent(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), title.Text, FindViewById<EditText>(Resource.Id.description).Text,date.Text, hours.Text, dropdown2.SelectedItem.ToString(), FindViewById<CheckBox>(Resource.Id.checkBox1).Checked, dropdown.SelectedItem.ToString(), Convert.ToInt32(FindViewById<EditText>(Resource.Id.editText1).Text), FindViewById<CheckBox>(Resource.Id.microphone).Checked, gametitle.Text);
                                if (eventCreated)
                                {
                                    dialog.Dismiss();
                                    Context context = view.Context;
                                    Intent intent = new Intent(context, typeof(MainActivity));
                                    context.StartActivity(intent);
                                }
                                else
                                {
                                    ShowSnack(view, GetString(Resource.String.AddQuickgame_NoCreated));
                                }
                            }
                            else
                            {
                                ShowSnack(view, GetString(Resource.String.AddQuickgame_NoHours));
                            }

                        }
                        else
                        {
                            ShowSnack(view, GetString(Resource.String.AddQuickgame_NoDate));
                        }

                    }
                    else
                    {
                        ShowSnack(view, GetString(Resource.String.AddQuickgame_NoTitle));
                    }

                }
                else
                {
                    ShowSnack(view, GetString(Resource.String.AddQuickgame_NoGame));
                }
                dialog.Dismiss();
            }
            else
            {
                ShowSnack(view, GetString(Resource.String.NoInternetConnection));
            }
        }

        public void CloseKey()
        {
            InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
            var currentFocus = Window.CurrentFocus;

            if (currentFocus != null)
            {
                inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
            }
        }

        private void ShowSnack(View view, string msg)
        {
            Snackbar snackbar1 = Snackbar.Make(view, msg, Snackbar.LengthShort);
            View snackBarView = snackbar1.View;
            snackBarView.SetBackgroundColor(Color.ParseColor("#333d59"));
            snackbar1.Show();
        }
    }
}