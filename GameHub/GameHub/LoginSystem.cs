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
using System.Json;
using Android.Graphics;

namespace GameHub
{
    [Activity(Label = "GameHub", MainLauncher = false, Icon = "@drawable/icon4", Theme = "@style/MyTheme", NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginSystem : AppCompatActivity
    {
        private static string LoginDataUser;
        private ISharedPreferences pref;
        View view;
        private Button mBtnSignUp;
        private Button mBtnSignIn;
        private Button go;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);

            pref = GetSharedPreferences(LoginDataUser, FileCreationMode.Private);
            AutoLog();

            view = FindViewById(Android.Resource.Id.Content);
            SetContentView(Resource.Layout.Login);
            Android.Support.V4.App.FragmentTransaction fragmentTransaction;
            View menulayout = FindViewById(Resource.Id.menulogin);
            LoginUP loginup = new LoginUP();
            LoginIN loginin = new LoginIN();
            mBtnSignUp = menulayout.FindViewById<Button>(Resource.Id.SignUpMenu);
            mBtnSignIn = menulayout.FindViewById<Button>(Resource.Id.SignInMenu);
            go = menulayout.FindViewById<Button>(Resource.Id.go);      

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
            go.Click += (object sender, EventArgs args) =>
            {
                
                Intent intent = new Intent(this, typeof(MainActivity));
                this.StartActivity(intent);
            };
        }

        private void ShowSnack(View view, string msg)
        {
            Snackbar snackbar1 = Snackbar.Make(view, msg, Snackbar.LengthShort);
            View snackBarView = snackbar1.View;
            snackBarView.SetBackgroundColor(Color.ParseColor("#333d59"));
            snackbar1.Show();
        }

        private async void AutoLog()
        {
            ProgressDialog dialog;
            if ((pref.GetString("PrefEmailUser", "") != null) && (pref.GetString("PrefPasswordUser", "") != null))
            {
                if ((pref.GetString("PrefEmailUser", "").Count() > 0) && (pref.GetString("PrefPasswordUser", "").Count() > 0))
                {
                    dialog = new ProgressDialog(this, Resource.Style.AppCompatAlertDialogStyle);
                    dialog.SetMessage(GetString(Resource.String.LoginLoading));
                    dialog.Show();

                    bool internetConnection = await API.checkForInternetConnection();

                    if (internetConnection)
                    {
                        bool authentificated = await API.authentification(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""));

                        if (authentificated)
                        {
                            Context context = view.Context;
                            Intent intent = new Intent(context, typeof(MainActivity));
                            context.StartActivity(intent);
                        }
                        else
                        {
                            ShowSnack(view, GetString(Resource.String.InvalidLoginOrPassword));
                        }
                    }
                    else
                    {
                        ShowSnack(view, GetString(Resource.String.NoInternetConnection));
                    }

                    dialog.Dismiss();
                }
            }
        }
    }
}