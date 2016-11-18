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

namespace GameHub.Fragments
{
    public class News : SupportFragment
    {

        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            mRecyclerView = inflater.Inflate(Resource.Layout.Fragment1, container, false) as RecyclerView;
            mLayoutManager = new LinearLayoutManager(mRecyclerView.Context);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mAdapter = new RecyclerAdapter();
            mRecyclerView.SetAdapter(mAdapter);

            return mRecyclerView;
        }

        public class RecyclerAdapter : RecyclerView.Adapter
        {
            public RecyclerAdapter()
            {
            }

            public class MyView : RecyclerView.ViewHolder
            {
                public MyView(View view) : base(view)
                {

                }
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.News, parent, false);
                MyView view = new MyView(row) { };
                return view;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                MyView view = holder as MyView;
            }

            public override int ItemCount
            {
                get { return 12; }  //iloœæ newsów w przysz³oœci JakasListaNewsów.Count
            }
        }



    }
}