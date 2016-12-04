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

namespace GameHub
{
    [Activity(Label = "Profile", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Profile : AppCompatActivity
    {
        ViewPager viewPager;
        TabLayout tabs;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

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


        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                    Toast.MakeText(this, "Zaznoczono: " + item.TitleFormatted,
    ToastLength.Short).Show();
                    return base.OnOptionsItemSelected(item);
            }

            return base.OnOptionsItemSelected(item);
        }

        private void SetUpViewPager(ViewPager viewPager)
        {
            TabAdapter adapter = new TabAdapter(SupportFragmentManager);
            adapter.AddFragment(new ProfileDescription(), GetString(Resource.String.tab_description));
            adapter.AddFragment(new ProfileAccounts(), GetString(Resource.String.tab_accounts));
            adapter.AddFragment(new ProfileFriends(), GetString(Resource.String.tab_friends));

            viewPager.Adapter = adapter;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Profile_ToolbarMenu, menu);
            return base.OnCreateOptionsMenu(menu);
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