using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GameHub.Fragments;
namespace GameHub
{
    [Activity(Label = "ProfileSettings")]
    public class ProfileSettings : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

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