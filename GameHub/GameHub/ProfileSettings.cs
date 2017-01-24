using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GameHub.Fragments;
using System.Text.RegularExpressions;
using Android.Graphics;

namespace GameHub
{
    [Activity(Label = "ProfileSettings")]
    public class ProfileSettings : AppCompatActivity
    {
        private LinkAccount currentLinkAccount;
        private Account currentAccount;
        private string LoginDataUser;
        private ISharedPreferences pref;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            pref = this.GetSharedPreferences(LoginDataUser, FileCreationMode.Private);

            SetContentView(Resource.Layout.ProfileSettings);

            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolBar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.Title = GetString(Resource.String.Settings_Title);
            Button buttone = FindViewById<Button>(Resource.Id.ProSetPicChange);
            Android.Support.V4.App.FragmentTransaction fragmentTransaction; 
            buttone.Click += (object sender, EventArgs args) =>
            {
                string Link = "https://gravatar.com/";
                Intent intent = new Intent(this, typeof(Browser));
                intent.PutExtra("BrowserLink", Link);
                OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                this.StartActivity(intent);
            };

            LoadAccount();
        }
        public async void LoadAccount()
        {
            bool internetConnection = await API.checkForInternetConnection();

            if (internetConnection)
            {
                currentAccount = await API.getAccountByEmail(pref.GetString("PrefEmailUser", ""));
                currentLinkAccount = await API.getLinkAccounts(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""));
                FindViewById<EditText>(Resource.Id.ProSetNick).Text = currentAccount.Login;
                FindViewById<EditText>(Resource.Id.ProSetEmail).Text = currentAccount.Email;
                FindViewById<EditText>(Resource.Id.ProSetCountry).Text = currentAccount.Language;
                FindViewById<EditText>(Resource.Id.ProSetPass).Text = "";
                FindViewById<EditText>(Resource.Id.ProSetDes).Text = currentAccount.Description;
                FindViewById<Switch>(Resource.Id.ProSetMicSwitch).Checked = currentAccount.Microphone;
                FindViewById<EditText>(Resource.Id.ProOrigin).Text = currentLinkAccount.ORIGIN_Account;
                FindViewById<EditText>(Resource.Id.ProSteam).Text = currentLinkAccount.STEAM_Account;
                FindViewById<EditText>(Resource.Id.ProBattle).Text = currentLinkAccount.BATTLE_Account;
                FindViewById<EditText>(Resource.Id.ProPSN).Text = currentLinkAccount.PSN_Account;
                FindViewById<EditText>(Resource.Id.ProXBOX).Text = currentLinkAccount.XBOX_Account;
                FindViewById<EditText>(Resource.Id.ProDiscord).Text = currentLinkAccount.DISCORD_Account;
                FindViewById<EditText>(Resource.Id.ProUplay).Text = currentLinkAccount.UPLAY_Account;
                FindViewById<EditText>(Resource.Id.ProSkype).Text = currentLinkAccount.SKYPE_Account;
                FindViewById<EditText>(Resource.Id.ProLOL).Text = currentLinkAccount.LOL_Account;
            }
            else
            {
                ShowSnack(GetString(Resource.String.NoInternetConnection));
            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.AcceptProfileChanges, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                case Resource.Id.menu_accept:
                    EditAccount();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }
        public async void EditAccount()
        {
            bool internetConnection = await API.checkForInternetConnection();

            if (internetConnection)
            {
                bool updated = false;

                if (FindViewById<EditText>(Resource.Id.ProSetNick).Text != currentAccount.Login)
                {
                    updated = await API.updateAccount(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), 3, FindViewById<EditText>(Resource.Id.ProSetNick).Text);
                }
                if (FindViewById<EditText>(Resource.Id.ProSetEmail).Text != currentAccount.Email)
                {
                    string email = FindViewById<EditText>(Resource.Id.ProSetEmail).Text;
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(email);
                    bool exist = await API.isAccountExist(email);

                    if (match.Success && !exist)
                    {
                        updated = await API.updateAccount(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), 3, email);
                    }
                    else
                    {
                        ShowSnack(GetString(Resource.String.InvalidEmail));
                    }
                }
                if (FindViewById<EditText>(Resource.Id.ProSetCountry).Text != currentAccount.Language)
                {
                    string lang = FindViewById<EditText>(Resource.Id.ProSetCountry).Text;
                    updated = await API.updateAccount(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), 9, lang);
                }
                if (FindViewById<EditText>(Resource.Id.ProSetPass).Text != "")
                {
                    string pass = FindViewById<EditText>(Resource.Id.ProSetCountry).Text;
                    if (pass.Length > 6)
                    {
                        updated = await API.updateAccount(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), 9, pass);
                    }
                    else
                    {
                        ShowSnack(GetString(Resource.String.PasswordTooShort));
                    }
                }
                if (FindViewById<EditText>(Resource.Id.ProSetDes).Text != currentAccount.Description)
                {
                    string desc = FindViewById<EditText>(Resource.Id.ProSetDes).Text;
                    updated = await API.updateAccount(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), 5, desc);
                }
                if (FindViewById<Switch>(Resource.Id.ProSetMicSwitch).Checked != currentAccount.Microphone)
                {
                    string mic = FindViewById<Switch>(Resource.Id.ProSetMicSwitch).Checked.ToString();
                    updated = await API.updateAccount(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), 6, mic);
                }
                if (FindViewById<EditText>(Resource.Id.ProOrigin).Text != currentLinkAccount.ORIGIN_Account)
                {
                    string origin = FindViewById<EditText>(Resource.Id.ProOrigin).Text;
                    updated = await API.updateAccount(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), 14, origin);
                }
                if (FindViewById<EditText>(Resource.Id.ProSteam).Text != currentLinkAccount.STEAM_Account)
                {
                    string steam = FindViewById<EditText>(Resource.Id.ProSteam).Text;
                    updated = await API.updateAccount(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), 13, steam);
                }
                if (FindViewById<EditText>(Resource.Id.ProBattle).Text != currentLinkAccount.BATTLE_Account)
                {
                    string batt = FindViewById<EditText>(Resource.Id.ProBattle).Text;
                    updated = await API.updateAccount(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), 17, batt);
                }
                if (FindViewById<EditText>(Resource.Id.ProPSN).Text != currentLinkAccount.PSN_Account)
                {
                    string psn = FindViewById<EditText>(Resource.Id.ProPSN).Text;
                    updated = await API.updateAccount(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), 11, psn);
                }
                if (FindViewById<EditText>(Resource.Id.ProXBOX).Text != currentLinkAccount.XBOX_Account)
                {
                    string xbox = FindViewById<EditText>(Resource.Id.ProXBOX).Text;
                    updated = await API.updateAccount(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), 12, xbox);
                }
                if (FindViewById<EditText>(Resource.Id.ProDiscord).Text != currentLinkAccount.DISCORD_Account)
                {
                    string dis = FindViewById<EditText>(Resource.Id.ProDiscord).Text;
                    updated = await API.updateAccount(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), 15, dis);
                }
                if (FindViewById<EditText>(Resource.Id.ProUplay).Text != currentLinkAccount.UPLAY_Account)
                {
                    string uplay = FindViewById<EditText>(Resource.Id.ProUplay).Text;
                    updated = await API.updateAccount(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), 16, uplay);
                }
                if (FindViewById<EditText>(Resource.Id.ProSkype).Text != currentLinkAccount.SKYPE_Account)
                {
                    string skype = FindViewById<EditText>(Resource.Id.ProSkype).Text;
                    updated = await API.updateAccount(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), 19, skype);
                }
                if (FindViewById<EditText>(Resource.Id.ProLOL).Text != currentLinkAccount.LOL_Account)
                {
                    string lol = FindViewById<EditText>(Resource.Id.ProLOL).Text;
                    updated = await API.updateAccount(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""), 18, lol);
                }

                if (updated)
                {
                    ShowSnack(GetString(Resource.String.UpdateSuccess));
                }
            }
            else
            {
                ShowSnack(GetString(Resource.String.NoInternetConnection));
            }
        }
        private void ShowSnack(string msg)
        {
            Snackbar snackbar1 = Snackbar.Make(FindViewById(Android.Resource.Id.Content), msg, Snackbar.LengthShort);
            View snackBarView = snackbar1.View;
            snackBarView.SetBackgroundColor(Color.ParseColor("#333d59"));
            snackbar1.Show();
        }

    }
}