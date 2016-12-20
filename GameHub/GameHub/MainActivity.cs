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

namespace GameHub
{
    [Activity(Label = "GameHub", ScreenOrientation = ScreenOrientation.Portrait) ]



    public class MainActivity : AppCompatActivity
    {
        private static string LoginDataUser;
        private ISharedPreferences pref;
        private DrawerLayout mDrawerLayout;
        private Android.Support.V7.App.ActionBarDrawerToggle drawerToggle;
        private SupportToolbar toolbar;
        private NavigationView navigationView;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            Intent intent;
            SupportFragment newFragment = new Hub();
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.flContent, newFragment, "Hub");
            trans.Commit();

            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            View navheader = navigationView.GetHeaderView(0);
            TextView profilename = (TextView)navheader.FindViewById(Resource.Id.textCustomer2);
            profilename.Text = "SeherimMaster";
            CircleImageView iv = (CircleImageView)navheader.FindViewById(Resource.Id.imageCustomerIcon);
            iv.Click += delegate
            {
                intent = new Intent(this, typeof(Profile));
                OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                this.StartActivity(intent);
            };

            navigationView.NavigationItemSelected += (sender, e) =>
            {
                
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
                        intent = new Intent(this, typeof(FriendList));
                        OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                        this.StartActivity(intent);

                        // ? Przejście na Activity
                        //intent = new Intent(this, typeof(Tournament));
                        //this.StartActivity(intent);

                        //OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);

                        break;

                    case (Resource.Id.nav_findplayers):
                        intent = new Intent(this, typeof(Chat));
                        OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                        this.StartActivity(intent);
                        

                        break;

                    case (Resource.Id.nav_profile):
                        intent = new Intent(this, typeof(Profile));
                        OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                        this.StartActivity(intent);
                        

                        break;

                    default:
                        break;
                }

                mDrawerLayout.CloseDrawers();





            };
        }

        //public void SendClickListener()
        //{
        //    intent = new Intent(this, typeof(Profile));
        //    OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
        //    this.StartActivity(intent);
       // }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbarMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent intent;
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    mDrawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
                    break;
                case Resource.Id.menut_ustawienia:
                    intent = new Intent(this, typeof(Settings));
                    OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                    this.StartActivity(intent);
                    break;
                case Resource.Id.MainToolbarLogout:
                    pref = GetSharedPreferences(LoginDataUser, FileCreationMode.Private);
                    ISharedPreferencesEditor PrefEdit = pref.Edit();
                    PrefEdit.Clear();
                    PrefEdit.Apply();
                    intent = new Intent(this, typeof(LoginSystem));
                    OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                    this.StartActivity(intent);
                    Finish();
                    break;
            }

            //Toast.MakeText(this, "Zaznaczone: " + item.TitleFormatted,
            //    ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);

        }


    }


}

