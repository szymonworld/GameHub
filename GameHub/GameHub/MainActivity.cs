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

namespace GameHub
{
    [Activity(Label = "GameHub", ScreenOrientation = ScreenOrientation.Portrait) ]



    public class MainActivity : AppCompatActivity
    {

        private DrawerLayout mDrawerLayout;
        private Android.Support.V7.App.ActionBarDrawerToggle drawerToggle;
        private SupportToolbar toolbar;
        private NavigationView navigationView;




        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            SupportFragment newFragment = new Hub();
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.flContent, newFragment, "Hub");
            trans.Commit();

            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);


            navigationView.NavigationItemSelected += (sender, e) =>
            {
                Intent intent;
                e.MenuItem.SetChecked(true);
                switch (e.MenuItem.ItemId)
                {
                    case (Resource.Id.nav_hub):
                        newFragment = new Hub();
                        trans = SupportFragmentManager.BeginTransaction();
                        trans.Replace(Resource.Id.flContent, newFragment, "Hub");
                        trans.AddToBackStack(null);
                        trans.Commit();

                        // ? Przejście na Activity
                        //Intent intent = new Intent(this, typeof(MainActivity));
                        //this.StartActivity(intent);

                        OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);

                        break;
                    case (Resource.Id.nav_friends):
                        newFragment = new Friends();
                        trans = SupportFragmentManager.BeginTransaction();
                        trans.Replace(Resource.Id.flContent, newFragment, "Znajomi");
                        trans.AddToBackStack(null);
                        trans.Commit();

                        // ? Przejście na Activity
                        //intent = new Intent(this, typeof(Tournament));
                        //this.StartActivity(intent);

                        OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);

                        break;
                        
                    case (Resource.Id.nav_findplayers):
                        intent = new Intent(this, typeof(Chat));
                        this.StartActivity(intent);

                        break;

                    case (Resource.Id.nav_profile):
                        intent = new Intent(this, typeof(Profile));
                        this.StartActivity(intent);

                        break;

                    default:
                        break;
                }

                mDrawerLayout.CloseDrawers();





            };
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbarMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    mDrawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }

            Toast.MakeText(this, "Zaznaczone: " + item.TitleFormatted,
                ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);

        }


    }


}

