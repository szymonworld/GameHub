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
using System.Threading.Tasks;
using Android.Support.V7.Widget;
using Android.Content.Res;
using Android.Util;
using GameHub.Helpers;
using Android.Graphics;

namespace GameHub.Fragments
{
    public class ProfileAccounts : SupportFragment
    {
        private RecyclerView mRecyclerView;
        private List<string> list = new List<string>();
        View view;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var values = GetRandomSubList(ConnectAccount.CheeseStrings, 5);
            mRecyclerView = inflater.Inflate(Resource.Layout.Friends, container, false) as RecyclerView;
            var mLayoutManager = new LinearLayoutManager(mRecyclerView.Context);

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(new SimpleStringRecyclerViewAdapter(mRecyclerView.Context, values, Activity.Resources));

            ((AppCompatActivity)this.Activity).SupportActionBar.Title = "Accounts";

            return mRecyclerView;


        }


        private List<string> GetRandomSubList(List<string> items, int amount)
        {
            List<string> list = new List<string>();
            Random random = new Random();
            while (list.Count < amount)
            {
                list.Add(items[random.Next(items.Count)]);
            }

            return list;
        }

        public class SimpleStringRecyclerViewAdapter : RecyclerView.Adapter
        {
            private readonly TypedValue mTypedValue = new TypedValue();
            private int mBackground;
            private List<string> mValues;
            private Dictionary<int, int> mCalculatedSizes;
            Resources mResource;

            public class MyView : RecyclerView.ViewHolder
            {
                public View mMainView { get; set; }
                public TextView Nick { get; set; }
                public Refractored.Controls.CircleImageView Platform { get; set; }

                public MyView(View view) : base(view)
                {
                    mMainView = view;
                }
            }
            public override int ItemCount
            {
                get
                {
                    return mValues.Count;
                }
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                //View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ProfileAccounts, parent, false);

                //TextView Nick = row.FindViewById<TextView>(Resource.Id.nick);
                //Refractored.Controls.CircleImageView platform = row.FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.EventPlatformIcon);

                //MyView view = new MyView(row) { Nick = Nick, Platform = platform };
                //return view;

                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ProfileAccounts, parent, false);
                view.SetBackgroundResource(mBackground);

                return new SimpleViewHolder(view);
            }


            public SimpleStringRecyclerViewAdapter(Context context, List<string> items, Resources res)
            {
                context.Theme.ResolveAttribute(Resource.Attribute.selectableItemBackground, mTypedValue, true);
                mBackground = mTypedValue.ResourceId;
                mValues = items;
                mResource = res;

                mCalculatedSizes = new Dictionary<int, int>();
            }

            //public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            //{
            //    var simpleHolder = holder as SimpleViewHolder;

            //    simpleHolder.mBoundString = mValues[position];
            //    simpleHolder.nick.Text = mValues[position];



            //}


            public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                var simpleHolder = holder as SimpleViewHolder;

                simpleHolder.mBoundString = mValues[position];
                simpleHolder.nick.Text = mValues[position];

                int drawableID = ConnectAccount.RandomAccount;
                BitmapFactory.Options options = new BitmapFactory.Options();

                if (mCalculatedSizes.ContainsKey(drawableID))
                {
                    options.InSampleSize = mCalculatedSizes[drawableID];
                }

                else
                {
                    options.InJustDecodeBounds = true;

                    BitmapFactory.DecodeResource(mResource, drawableID, options);

                    options.InSampleSize = ConnectAccount.CalculateInSampleSize(options, 100, 100);
                    options.InJustDecodeBounds = false;

                    mCalculatedSizes.Add(drawableID, options.InSampleSize);
                }


                var bitMap = await BitmapFactory.DecodeResourceAsync(mResource, drawableID, options);

                simpleHolder.platform.SetImageBitmap(bitMap);
            }


        }
        public class SimpleViewHolder : RecyclerView.ViewHolder
        {
            public string mBoundString;
            public TextView nick;
            public ImageView platform;
            public View mView;

            public SimpleViewHolder(View view) : base(view)
            {
                mView = view;
                platform = view.FindViewById<ImageView>(Resource.Id.Icon_platform);
                nick = view.FindViewById<TextView>(Resource.Id.nick);
            }

            public override string ToString()
            {
                return base.ToString() + " '" + nick.Text;
            }
        }
    }
}