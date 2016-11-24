using System;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using SupportFragment = Android.Support.V4.App.Fragment;
using System.Collections.Generic;
using Android.Graphics;
using Android.Util;
using Android.Content;
using Android.Content.Res;
using Android.Widget;
using GameHub.Lists;
using Android.Support.Design.Widget;

namespace GameHub.Fragments
{
    public class Friends : SupportFragment
    {

        private List<string> list = new List<string>();
        private RecyclerView.Adapter mAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            


            RecyclerView recyclerView = inflater.Inflate(Resource.Layout.Friends, container, false) as RecyclerView;

            GetRandomSubList(Lists.Friends.NickStrings, 12);  // dodanie 12 obiektów do listy 

            var mLayoutManager = new LinearLayoutManager(recyclerView.Context);
            var onScrollListener = new RecyclerViewOnScrollListener(mLayoutManager);
            recyclerView.AddOnScrollListener(onScrollListener);
            recyclerView.SetLayoutManager(mLayoutManager);
            mAdapter = new SimpleStringRecyclerViewAdapter(recyclerView.Context, list, Activity.Resources);
            recyclerView.SetAdapter(mAdapter);

            // Dodanie nowych obiektów do listy po dojechaniu na dó³
            onScrollListener.LoadMoreEvent += (object sender, EventArgs e) => {

                GetRandomSubList(Lists.Friends.NickStrings, 5); // generowanie i dodanie noeych obiektów

                mAdapter.NotifyDataSetChanged(); 
            };

            //NIE DZIA£A
            //var view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.Hub, container, false);

            //AppBarLayout appBarLayout = view.FindViewById<AppBarLayout>(Resource.Id.appbar);
            //appBarLayout.SetExpanded(true, true);

            return recyclerView;
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
            Resources mResource;
            private Dictionary<int, int> mCalculatedSizes;

            public SimpleStringRecyclerViewAdapter(Context context, List<string> items, Resources res)
            {
                context.Theme.ResolveAttribute(Resource.Attribute.selectableItemBackground, mTypedValue, true);
                mBackground = mTypedValue.ResourceId;
                mValues = items;
                mResource = res;

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

                    BitmapFactory.DecodeResource(mResource, drawableID, options);

                    options.InSampleSize = Lists.Friends.CalculateInSampleSize(options, 100, 100);
                    options.InJustDecodeBounds = false;

                    mCalculatedSizes.Add(drawableID, options.InSampleSize);
                }


                var bitMap = await BitmapFactory.DecodeResourceAsync(mResource, drawableID, options);

                simpleHolder.mImageView.SetImageBitmap(bitMap);
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