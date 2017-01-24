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
        private string mDescription;
        private View mView;

        public string Description
        {
            get
            {
                return mDescription;
            }
            set
            {
                mDescription = value;
                UpdateDescription(value);
            }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            mView = inflater.Inflate(Resource.Layout.ProfileDescription, container, false);
            Description = mDescription;
            return mView;

        }

        private void UpdateDescription(string desc)
        {
            if (mView != null)
            { 
                TextView Description = mView.FindViewById<TextView>(Resource.Id.Description);
                Description.Text = desc;
            }
        }
    }
}