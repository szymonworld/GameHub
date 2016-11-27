using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using System.Collections.Generic;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

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
            ((AppCompatActivity)this.Activity).SetSupportActionBar(toolbar);
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
            TabAdapter adapter = new TabAdapter(ChildFragmentManager, this);
            adapter.AddFragment(new News(), "Wiadomości", (int)Resource.Drawable.ic_gamepad_white_24dp);
            adapter.AddFragment(new Wydarzenia(), "Wydarzenia", (int)Resource.Drawable.ic_timelapse_white_24dp);

            viewPager.Adapter = adapter;
            viewPager.AddOnPageChangeListener(new MyPageChangeListener(this, adapter.FragmentNames));
        }

        public void Title(string title)
        {
            ((AppCompatActivity)this.Activity).SupportActionBar.Title = title;
        }

    }

    class MyPageChangeListener : Java.Lang.Object, ViewPager.IOnPageChangeListener
    {
        private Hub currentHub;
        private List<string> fragmentNames;

        public MyPageChangeListener(Hub currentHub, List<string> fragmentNames)
        {
            this.currentHub = currentHub;
            this.fragmentNames = fragmentNames;
        }
        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
        }

        public void OnPageScrollStateChanged(int position)
        {
        }
        public void OnPageSelected(int position)
        {
            currentHub.Title(fragmentNames[position]);
        }
    }

    public class TabAdapter : FragmentPagerAdapter
    {
        protected Hub hub;
        public string title;
        public List<SupportFragment> Fragments { get; set; }
        public List<string> FragmentNames { get; set; }
        public List<int> FragmentIcon { get; set; }

        public TabAdapter(SupportFragmentManager sfm, Hub hub) : base(sfm)
        {
            Fragments = new List<SupportFragment>();
            FragmentNames = new List<string>();
            FragmentIcon = new List<int>();
            this.hub = hub;
        }

        public void AddFragment(SupportFragment fragment, string name, int icon)
        {
            Fragments.Add(fragment);
            FragmentNames.Add(name);
            FragmentIcon.Add(icon);
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