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

            login.Click += (object sender, EventArgs args) =>
            {
                Context context = view.Context;
                Intent intent = new Intent(context, typeof(MainActivity));
                context.StartActivity(intent);
            };


            return view;

        }
    }
}