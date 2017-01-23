using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Graphics;
using Android.Support.V7.App;

namespace GameHub
{
    public class SimpleStringRecyclerViewAdapter : RecyclerView.Adapter
    {
        private readonly TypedValue mTypedValue = new TypedValue();
        private int mBackground;
        private List<string> mValues;
        public int isFriend;
        public event EventHandler<int> ItemClick;
        public View mainview;
        //private Context cc;
        private Android.Support.V7.App.AlertDialog.Builder builder;
        private Dictionary<int, int> mCalculatedSizes;
        private int uri;

        public SimpleStringRecyclerViewAdapter(Context context, List<string> items, int resource, View mView, int friend = 3)
        {
            //context.Theme.ResolveAttribute(Resource.Attribute.colorPrimary, mTypedValue, true);
            mBackground = mTypedValue.ResourceId;
            mValues = items;
            uri = resource;
            mainview = mView;
            isFriend = friend;

            //mResource = res;

            mCalculatedSizes = new Dictionary<int, int>();
        }

        public override int ItemCount
        {
            get
            {
                return mValues.Count;
            }
        }
        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }

        public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var simpleHolder = holder as SimpleViewHolder;
            simpleHolder.mBoundString = mValues[position];
            simpleHolder.mTxtView.Text = mValues[position];

            int drawableID = Lists.Friends.RandomNick;
            BitmapFactory.Options options = new BitmapFactory.Options();

            if (mCalculatedSizes.ContainsKey(drawableID))
            {
                options.InSampleSize = mCalculatedSizes[drawableID];
            }

            else
            {
                options.InJustDecodeBounds = true;



                options.InSampleSize = Lists.Friends.CalculateInSampleSize(options, 100, 100);
                options.InJustDecodeBounds = false;

                mCalculatedSizes.Add(drawableID, options.InSampleSize);
            }


            //var bitMap = await BitmapFactory.DecodeResourceAsync(mResource, drawableID, options);

            simpleHolder.mImageView.SetImageResource(uri);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.List_Item, parent, false);
            //view.SetBackgroundResource(mBackground);
            
            builder = new Android.Support.V7.App.AlertDialog.Builder(view.Context);
            //cc = view.Context;
            return new SimpleViewHolder(view, OnClick, mainview, isFriend);
        }
        public class SimpleViewHolder : RecyclerView.ViewHolder
        {
            public string mBoundString;
            public readonly View mView;
            public readonly ImageView mImageView;
            public readonly ImageView mImageViewChat;
            public readonly TextView mTxtView;
            public View moryginalView;

            public SimpleViewHolder(View view, Action<int> listener, View mainView, int isFriend) : base(view)
            {
                mView = view;
                mImageView = view.FindViewById<ImageView>(Resource.Id.avatar);
                mTxtView = view.FindViewById<TextView>(Resource.Id.text1);
                mImageViewChat = view.FindViewById<ImageView>(Resource.Id.chat_icon);

                if(isFriend == 3)
                {
                    Random rand = new Random();
                    isFriend = rand.Next(0, 2);
                }

                //mImageView.Pressed = false;
                if(isFriend == 0)
                {
                    mImageViewChat.Click += (sender, e) =>
                    {
                        Context context = mainView.Context;
                        Intent intent = new Intent(context, typeof(Chat));
                        intent.PutExtra("Name", mTxtView.Text);
                        context.StartActivity(intent);
                    };

                }
                else
                {
                    
                    mImageViewChat.SetImageResource(Resource.Drawable.ic_plus_circle_white_48dp);
                    mImageViewChat.Click += (sender, e) =>
                    {
                        DialogMenager(mainView.Context);
                    };
                }

               
                
                view.Click += (sender, e) => listener(Position);
            }

            public override string ToString()
            {
                return base.ToString() + " '" + mTxtView.Text;
            }

            public void DialogMenager(Context cc)
            {

                var builder = new Android.Support.V7.App.AlertDialog.Builder(cc, Resource.Style.DarkThemeDialog);
                //var dialogview =  LayoutInflater.Inflate(Resource.Layout.AddFriendDialog, null);
                builder.SetView(Resource.Layout.AddFriendDialog);

                builder.SetPositiveButton("AKCEPTUJ", (EventHandler<DialogClickEventArgs>)null);
                builder.SetNegativeButton("ODRZUÆ", (EventHandler<DialogClickEventArgs>)null);
                var dialog = builder.Create();
                dialog.Show();
                var PositiveButton = dialog.GetButton((int)DialogButtonType.Positive);
                PositiveButton.Click += delegate
                {
                    dialog.Cancel();
                };
                var NegativeButton = dialog.GetButton((int)DialogButtonType.Positive);
                NegativeButton.Click += delegate
                {
                    dialog.Cancel();
                };
            }
        }
    }
}