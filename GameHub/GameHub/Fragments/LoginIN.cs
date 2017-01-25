using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Json;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Graphics;
using Android.Views.InputMethods;

namespace GameHub.Fragments
{
    public class LoginIN : SupportFragment
    {
        private static string LoginDataUser;
        private ISharedPreferences pref;
        View view;
        EditText password;
        EditText email;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.LoginIN, container, false);

            pref = ((AppCompatActivity)this.Activity).GetSharedPreferences(LoginDataUser, FileCreationMode.Private);
            RestoreData();
            ProgressDialog dialog;
            Button login = view.FindViewById<Button>(Resource.Id.LoginIn_ButtonLogin);

            TextView lostpassword = view.FindViewById<TextView>(Resource.Id.lostpass);
            lostpassword.Click += delegate {
                var uri = Android.Net.Uri.Parse("http://www.google.com");
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            };

            login.Click += async (object sender, EventArgs args) =>
            {
                InputMethodManager inputManager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
                var currentFocus = Activity.CurrentFocus;
                if (currentFocus != null)
                {
                    inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
                }

                bool internetConnection = await API.checkForInternetConnection();
                dialog = new ProgressDialog(this.Activity, Resource.Style.AppCompatAlertDialogStyle);
                dialog.SetMessage(GetString(Resource.String.LoginLoading));
                dialog.Show();
                if (internetConnection)
                {
                    email = view.FindViewById<EditText>(Resource.Id.input_Email_In);
                    password = view.FindViewById<EditText>(Resource.Id.input_Password_In);

                    bool authentificated = await API.authentification(email.Text, password.Text);

                    if (authentificated)
                    {
                        SaveData();
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
            };


            return view;

        }

        private void SaveData()
        {
            ISharedPreferencesEditor PrefEdit = pref.Edit();
            PrefEdit.PutString("PrefEmailUser", email.Text.ToString());
            PrefEdit.PutString("PrefPasswordUser", password.Text.ToString());
            PrefEdit.Apply(); 
        }
        private void RestoreData()
        {
            email = view.FindViewById<EditText>(Resource.Id.input_Email_In);
            password = view.FindViewById<EditText>(Resource.Id.input_Password_In);
            email.Text = pref.GetString("PrefEmailUser", "");
            password.Text = pref.GetString("PrefPasswordUser", "");
            
        }
        private void ShowSnack(View view, string msg)
        {
            Snackbar snackbar1 = Snackbar.Make(view, msg, Snackbar.LengthShort);
            View snackBarView = snackbar1.View;
            snackBarView.SetBackgroundColor(Color.ParseColor("#333d59"));
            snackbar1.Show();

        }
    }
}