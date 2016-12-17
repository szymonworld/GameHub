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

namespace GameHub.Fragments
{
    public class LoginIN : SupportFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.LoginIN, container, false);
            Button login = view.FindViewById<Button>(Resource.Id.LoginIn_ButtonLogin);

            login.Click += async(object sender, EventArgs args) =>
            {
                bool internetConnection = await API.checkForInternetConnection();

                if (internetConnection)
                {
                    EditText email = view.FindViewById<EditText>(Resource.Id.input_Email_In);
                    EditText password = view.FindViewById<EditText>(Resource.Id.input_Password_In);

                    bool authentificated = await API.authentification(email.Text, password.Text);

                    if (authentificated)
                    {
                            Context context = view.Context;
                            Intent intent = new Intent(context, typeof(MainActivity));
                            context.StartActivity(intent);
                    }
                    else
                    {
                        Toast.MakeText(view.Context, GetString(Resource.String.InvalidLoginOrPassword), ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(view.Context, GetString(Resource.String.NoInternetConnection), ToastLength.Short).Show();
                }
            };


            return view;

        }
    }
}