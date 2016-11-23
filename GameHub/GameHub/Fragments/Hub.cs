using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.Widget;
using Android.Views;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Widget;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using Android.Support.Design.Widget;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.App;
using Android.Views;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Content;
using Android.Support.V4.View;
using Android.Support.V4.App;
using System.Collections.Generic;
using Java.Lang;
using GameHub.Fragments;

namespace GameHub.Fragments
{
    public class Hub : SupportFragment
    {
        ViewPager viewPager;
        TabLayout tabs;
        SupportToolbar toolbar;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.Hub, container, false);

            toolbar = view.FindViewById<SupportToolbar>(Resource.Id.toolbar);
            
            tabs = view.FindViewById<TabLayout>(Resource.Id.tabsHub);
            viewPager = view.FindViewById<ViewPager>(Resource.Id.viewpagerHub);

            SetUpViewPager(viewPager);

            tabs.SetupWithViewPager(viewPager);

            return view;

            
        }
        private void SetUpViewPager(ViewPager viewPager)
        {
            TabAdapter adapter = new TabAdapter(ChildFragmentManager);
            adapter.AddFragment(new News(), "Gaming News");
            adapter.AddFragment(new Friends(), "Wydarzenia");

            viewPager.Adapter = adapter;
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