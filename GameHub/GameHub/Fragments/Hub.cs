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
            ((AppCompatActivity) this.Activity).SetSupportActionBar(toolbar);
            ((AppCompatActivity)this.Activity).SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            ((AppCompatActivity)this.Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            ((AppCompatActivity)this.Activity).SupportActionBar.Title = "HUB";

            tabs = view.FindViewById<TabLayout>(Resource.Id.tabsHub);

            
            
            viewPager = view.FindViewById<ViewPager>(Resource.Id.viewpagerHub);

            SetUpViewPager(viewPager);

            tabs.SetupWithViewPager(viewPager);

            return view;

            
        }
        private void SetUpViewPager(ViewPager viewPager)
        {
            TabAdapter adapter = new TabAdapter(ChildFragmentManager);
            adapter.AddFragment(new News(), "Wiadomoœci", (int) Resource.Drawable.ic_gamepad_white_24dp);
            adapter.AddFragment(new Friends(), "Wydarzenia",(int) Resource.Drawable.ic_timelapse_white_24dp);

            viewPager.Adapter = adapter;
        }

    }
    public class TabAdapter : FragmentPagerAdapter
    {
        public List<SupportFragment> Fragments { get; set; }
        public List<string> FragmentNames { get; set; }
        public List<int> FragmentIcon { get; set; }

        //public int FragmentIcon;

        public TabAdapter(SupportFragmentManager sfm) : base(sfm)
        {
            Fragments = new List<SupportFragment>();
            FragmentNames = new List<string>();
            FragmentIcon = new List<int>();
        }

        public void AddFragment(SupportFragment fragment, string name, int icon)
        {
            Fragments.Add(fragment);
            FragmentNames.Add(name);
            FragmentIcon.Add(icon);
            //FragmentIcon = icon;
            
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

        //public override ICharSequence GetPageTitleFormatted(int position)
        //{
        //    return new Java.Lang.String(FragmentNames[position]);
        //}

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            var image = Application.Context.Resources.GetDrawable(FragmentIcon[position]);
            image.SetBounds(0, 0, image.IntrinsicWidth, image.IntrinsicHeight);

            var spannableString = new Android.Text.SpannableString("[icon]");
            var imageSpan = new Android.Text.Style.ImageSpan(image, Android.Text.Style.SpanAlign.Bottom);
            spannableString.SetSpan(imageSpan, 0, 1, Android.Text.SpanTypes.ExclusiveExclusive);
            return spannableString;
        }
    }
}