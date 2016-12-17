using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Graphics;
using GameHub.Fragments;
using Android.Util;
using Android.Content.Res;
using Android.Support.Design.Widget;

namespace GameHub
{
    [Activity(Label = "FriendList")]
    public class FriendList : AppCompatActivity
    {

        private List<string> list = new List<string>();
        private RecyclerView.Adapter mAdapter;
        private RecyclerView mRecyclerView;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.FriendList);
            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolBar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.Title = GetString(Resource.String.Friends_Title);

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerview);

            int uri2 = Resource.Drawable.Icon2;
            GetRandomSubList(Lists.Friends.NickStrings, 2);  // dodanie 30 obiektów do listy 

            var mLayoutManager = new LinearLayoutManager(mRecyclerView.Context);
            var onScrollListener = new RecyclerViewOnScrollListener(mLayoutManager);
            mRecyclerView.AddOnScrollListener(onScrollListener);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mAdapter = new SimpleStringRecyclerViewAdapter(mRecyclerView.Context, list, uri2);
            mRecyclerView.SetAdapter(mAdapter);

            FloatingActionButton myFab = (FloatingActionButton)FindViewById(Resource.Id.fab);
            myFab.Click += delegate
            {
                GetRandomSubList(Lists.Friends.NickStrings, 1);
                if (uri2 == Resource.Drawable.Icon2)
                {
                    uri2 = Resource.Drawable.acceptsmall;
                }
                else
                {
                    uri2 = Resource.Drawable.Icon2;
                }
                mAdapter = new SimpleStringRecyclerViewAdapter(mRecyclerView.Context, list, uri2);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();
            };

            // Dodanie nowych obiektów do listy po dojechaniu na dó³
            onScrollListener.LoadMoreEvent += (object sender, EventArgs e) => {

                //GetRandomSubList(Lists.Friends.NickStrings, 5); // generowanie i dodanie noeych obiektów

                //mAdapter.NotifyDataSetChanged();
            };

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void GetRandomSubList(List<string> items, int a)
        {
            //List<string> list = new List<string>();
            Random random = new Random();
            while (a > 0)
            {
                list.Add(items[random.Next(items.Count)]);
                a--;
            }
            //return list;
        }

        public class SimpleStringRecyclerViewAdapter : RecyclerView.Adapter
        {
            private readonly TypedValue mTypedValue = new TypedValue();
            private int mBackground;
            private List<string> mValues;
            private Dictionary<int, int> mCalculatedSizes;
            private int uri;

            public SimpleStringRecyclerViewAdapter(Context context, List<string> items, int resource)
            {
                context.Theme.ResolveAttribute(Resource.Attribute.selectableItemBackground, mTypedValue, true);
                mBackground = mTypedValue.ResourceId;
                mValues = items;
                uri = resource;
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
                view.SetBackgroundResource(mBackground);

                return new SimpleViewHolder(view);
            }
        }
        public class SimpleViewHolder : RecyclerView.ViewHolder
        {
            public string mBoundString;
            public readonly View mView;
            public readonly ImageView mImageView;
            public readonly TextView mTxtView;

            public SimpleViewHolder(View view) : base(view)
            {
                mView = view;
                mImageView = view.FindViewById<ImageView>(Resource.Id.avatar);
                mTxtView = view.FindViewById<TextView>(Resource.Id.text1);
            }

            public override string ToString()
            {
                return base.ToString() + " '" + mTxtView.Text;
            }
        }
    }
}