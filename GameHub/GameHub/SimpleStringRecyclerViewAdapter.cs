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
using Android.Support.V7.Widget;
using Android.Util;
using Android.Graphics;

namespace GameHub
{
    public class SimpleStringRecyclerViewAdapter : RecyclerView.Adapter
    {
        private readonly TypedValue mTypedValue = new TypedValue();
        private int mBackground;
        private List<string> mValues;
        public event EventHandler<int> ItemClick;
        public View Mainview;
        private Dictionary<int, int> mCalculatedSizes;
        private int uri;

        public SimpleStringRecyclerViewAdapter(Context context, List<string> items, int resource, View mView)
        {
            //context.Theme.ResolveAttribute(Resource.Attribute.colorPrimary, mTypedValue, true);
            mBackground = mTypedValue.ResourceId;
            mValues = items;
            uri = resource;
            Mainview = mView;

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


            return new SimpleViewHolder(view, OnClick, Mainview);
        }
        public class SimpleViewHolder : RecyclerView.ViewHolder
        {
            public string mBoundString;
            public readonly View mView;
            public readonly ImageView mImageView;
            public readonly ImageView mImageViewChat;
            public readonly TextView mTxtView;
            public View moryginalView;

            public SimpleViewHolder(View view, Action<int> listener, View MainView) : base(view)
            {
                mView = view;
                mImageView = view.FindViewById<ImageView>(Resource.Id.avatar);
                mTxtView = view.FindViewById<TextView>(Resource.Id.text1);
                mImageViewChat = view.FindViewById<ImageView>(Resource.Id.chat_icon);
                moryginalView = MainView;
                //mImageView.Pressed = false;

                mImageViewChat.Click += (sender, e) =>
                {
                    Context context = moryginalView.Context;
                    Intent intent = new Intent(context, typeof(Chat));
                    intent.PutExtra("Name", mTxtView.Text);
                    context.StartActivity(intent);
                };

                view.Click += (sender, e) => listener(Position);
            }

            public override string ToString()
            {
                return base.ToString() + " '" + mTxtView.Text;
            }
        }
    }
}