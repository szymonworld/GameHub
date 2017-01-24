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
        private View mView;
        private LinkAccount mLinkedAccounts;
        public LinkAccount LinkedAccounts
        {
            get
            {
                return mLinkedAccounts;
            }
            set
            {
                mLinkedAccounts = value;
                UpdateIcons();
            }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            mRecyclerView = inflater.Inflate(Resource.Layout.Friends, container, false) as RecyclerView;
            ((AppCompatActivity)this.Activity).SupportActionBar.Title = "Accounts";

            return mRecyclerView;
        }

        private void UpdateIcons()
        {
            var mLayoutManager = new LinearLayoutManager(mRecyclerView.Context);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(new SimpleStringRecyclerViewAdapter(mRecyclerView.Context, LinkedAccounts, Activity.Resources));
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
            private List<int> mTypes;
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
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ProfileAccounts, parent, false);
                view.SetBackgroundResource(mBackground);

                return new SimpleViewHolder(view);
            }

            private List<string> ConvertAccountsToList(LinkAccount acc)
            {

                List<string> list = new List<string>();
                mTypes = new List<int>();

                if (acc != null)
                {
                    if (acc.PSN_Account != null && acc.PSN_Account.Length > 4)
                    {
                        list.Add(acc.PSN_Account);
                        mTypes.Add(0);
                    }
                    if (acc.XBOX_Account != null && acc.XBOX_Account.Length > 4)
                    {
                        list.Add(acc.XBOX_Account);
                        mTypes.Add(1);
                    }
                    if (acc.STEAM_Account != null && acc.STEAM_Account.Length > 4)
                    {
                        list.Add(acc.STEAM_Account);
                        mTypes.Add(2);
                    }
                    if (acc.ORIGIN_Account != null && acc.ORIGIN_Account.Length > 4)
                    {
                        list.Add(acc.ORIGIN_Account);
                        mTypes.Add(3);
                    }
                    if (acc.DISCORD_Account != null && acc.DISCORD_Account.Length > 4)
                    {
                        list.Add(acc.DISCORD_Account);
                        mTypes.Add(4);
                    }
                    if (acc.UPLAY_Account != null && acc.UPLAY_Account.Length > 4)
                    {
                        list.Add(acc.UPLAY_Account);
                        mTypes.Add(5);
                    }
                    if (acc.BATTLE_Account != null && acc.BATTLE_Account.Length > 4)
                    {
                        list.Add(acc.BATTLE_Account);
                        mTypes.Add(6);
                    }
                    if (acc.LOL_Account != null && acc.LOL_Account.Length > 4)
                    {
                        list.Add(acc.LOL_Account);
                        mTypes.Add(7);
                    }
                    if (acc.SKYPE_Account != null && acc.SKYPE_Account.Length > 4)
                    {
                        list.Add(acc.SKYPE_Account);
                        mTypes.Add(8);
                    }
                }

                return list; 
            }

            public SimpleStringRecyclerViewAdapter(Context context, LinkAccount items, Resources res)
            {
                context.Theme.ResolveAttribute(Resource.Attribute.selectableItemBackground, mTypedValue, true);
                mBackground = mTypedValue.ResourceId;
                mValues = ConvertAccountsToList(items);
                mResource = res;

                mCalculatedSizes = new Dictionary<int, int>();
            }
            private int GetIcon(int pos)
            {

                switch (pos)
                {
                    case 0:
                        return Resource.Drawable.Icon_psn;
                    break;
                    case 1:
                        return Resource.Drawable.Icon_xbox;
                    break;
                    case 2:
                        return Resource.Drawable.Icon_steam;
                    break;
                    case 3:
                        return Resource.Drawable.Icon_origin;
                    break;
                    case 4:
                        return Resource.Drawable.Icon_discord;
                    break;
                    case 5:
                        return Resource.Drawable.Icon_uplay;
                    break;
                    case 6:
                        return Resource.Drawable.Icon_battle;
                    break;
                    case 7:
                        return Resource.Drawable.Icon_lol;
                    break;
                    case 8:
                        return Resource.Drawable.Icon_skype;
                    break;
                    default:
                        return -1;
                    break;
                }
            }

            public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                var simpleHolder = holder as SimpleViewHolder;

                simpleHolder.mBoundString = mValues[position];
                simpleHolder.nick.Text = mValues[position];

                int drawableID = GetIcon(mTypes[position]);
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