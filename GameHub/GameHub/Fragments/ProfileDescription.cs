using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace GameHub.Fragments
{
    public class ProfileDescription : SupportFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ProfileDescription, container, false);
            TextView Description = view.FindViewById<TextView>(Resource.Id.Description);
            Description.Text = "To jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis graczaTo jest opis gracza";



            return view;

        }
    }
}