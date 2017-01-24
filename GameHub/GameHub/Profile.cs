using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Content;
using System;
using System.Collections.Generic;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Widget;
using Android.Content.PM;
using Android.Support.V4.View;
using GameHub.Fragments;
using Java.Lang;
using Android.Graphics;

namespace GameHub
{
    [Activity(Label = "Profile", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Profile : AppCompatActivity
    {
        private ViewPager viewPager;
        private TabLayout tabs;
        private LinkAccount currentLinkAccount;
        private Account currentAccount;
        private string LoginDataUser;
        private ISharedPreferences pref;
        private ProfileDescription mProfileDescription;
        private ProfileAccounts mProfileAccounts;
        private ProfileFriends mProfileFriends;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            pref = ((AppCompatActivity)this).GetSharedPreferences(LoginDataUser, FileCreationMode.Private);

            SetContentView(Resource.Layout.Profile);
            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolBar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.Title = "Profil";

            CollapsingToolbarLayout collapsingToolBar = FindViewById<CollapsingToolbarLayout>(Resource.Id.collapsing_toolbar);
            tabs = FindViewById<TabLayout>(Resource.Id.tabsHub);
            viewPager = FindViewById<ViewPager>(Resource.Id.viewpagerHub);
            SetUpViewPager(viewPager);
            tabs.SetupWithViewPager(viewPager);
            LoadAccount();
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.ProfilToolbarEdit:
                    Intent intent = new Intent(this, typeof(ProfileSettings));
                    OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                    this.StartActivity(intent);
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void SetUpViewPager(ViewPager viewPager)
        {
            TabAdapter adapter = new TabAdapter(SupportFragmentManager);
            mProfileDescription = new ProfileDescription();
            mProfileAccounts = new ProfileAccounts();
            mProfileFriends = new ProfileFriends();
            adapter.AddFragment(mProfileDescription, GetString(Resource.String.tab_description));
            adapter.AddFragment(mProfileAccounts, GetString(Resource.String.tab_accounts));
            adapter.AddFragment(mProfileFriends, GetString(Resource.String.tab_friends));

            viewPager.Adapter = adapter;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Profile_ToolbarMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        protected override void OnResume()
        {
            base.OnResume();
            LoadAccount();
        }
        public async void LoadAccount()
        {
            bool internetConnection = await API.checkForInternetConnection();

            if (internetConnection)
            {
                currentAccount = await API.getAccountByEmail(pref.GetString("PrefEmailUser", ""));
                currentLinkAccount = await API.getLinkAccounts(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""));
                mProfileDescription.Description = currentAccount.Description;
                mProfileAccounts.LinkedAccounts = currentLinkAccount;
                FindViewById<TextView>(Resource.Id.textViewWydarzenia1).Text = currentAccount.Login;
                FindViewById<TextView>(Resource.Id.pro_country).Text = currentAccount.Language;
                FindViewById<TextView>(Resource.Id.pro_reputation).Text = currentAccount.RepPoint.ToString();
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
        public class TabAdapter : FragmentPagerAdapter
        {
        public List<SupportFragment> Fragments { get; set; }
        public List<string> FragmentNames { get; set; }

        public TabAdapter(SupportFragmentManager sfm) : base(sfm)
        {
            Fragments = new List<SupportFragment>();
            FragmentNames = new List<string>();
        }

        public void AddFragment(SupportFragment fragment, string name)
        {
            Fragments.Add(fragment);
            FragmentNames.Add(name);
        }

        public override int Count
        {
            get
            {
                return Fragments.Count;
            }
        }

        public override SupportFragment GetItem(int position)
        {
            return Fragments[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(FragmentNames[position]);
        }
    }
}