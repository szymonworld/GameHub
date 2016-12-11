using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using Android.Support.Design.Widget;
using Android.Content;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using Android.Content.PM;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using GameHub.Fragments;

namespace GameHub
{
    [Activity(Label = "GameHub", MainLauncher = false, Icon = "@drawable/icon4", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginSystem : AppCompatActivity
    {
        private Button mBtnSignUp;
        private Button mBtnSignIn;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Login);
            Android.Support.V4.App.FragmentTransaction fragmentTransaction;
            View menulayout = FindViewById(Resource.Id.menulogin);
            LoginUP loginup = new LoginUP();
            LoginIN loginin = new LoginIN();
            mBtnSignUp = menulayout.FindViewById<Button>(Resource.Id.SignUpMenu);
            mBtnSignIn = menulayout.FindViewById<Button>(Resource.Id.SignInMenu);

            mBtnSignUp.Click += (object sender, EventArgs args) =>
            {
                fragmentTransaction = SupportFragmentManager.BeginTransaction();
                fragmentTransaction.Remove(loginup);
                fragmentTransaction.Add(Resource.Id.LoginFrame, loginup);
                fragmentTransaction.AddToBackStack(null);
                fragmentTransaction.Commit();
            };

            mBtnSignIn.Click += (object sender, EventArgs args) =>
            {
                fragmentTransaction = SupportFragmentManager.BeginTransaction();
                fragmentTransaction.Remove(loginin);
                fragmentTransaction.Add(Resource.Id.LoginFrame, loginin);
                fragmentTransaction.AddToBackStack(null);
                fragmentTransaction.Commit();
            };
        }
    }
}