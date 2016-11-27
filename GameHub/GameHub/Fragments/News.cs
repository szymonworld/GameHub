using System;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using SupportFragment = Android.Support.V4.App.Fragment;
using System.Collections.Generic;
using GameHub.Lists;
using Android.Graphics;
using Android.Util;
using Android.Content;
using Android.Content.Res;
using Android.Widget;
using DesignLibrary.Helpers;
using Android.Support.V7.App;

namespace GameHub.Fragments
{
    public class News : SupportFragment
    {

        private RecyclerView mRecyclerView;
        private RecyclerView.Adapter mAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            int iloscnewsow = 12; //Startowa iloœæ newsóe
            mRecyclerView = inflater.Inflate(Resource.Layout.Friends, container, false) as RecyclerView;
            var mLayoutManager = new LinearLayoutManager(mRecyclerView.Context);
            mAdapter = new RecyclerAdapter(iloscnewsow);

            var onScrollListener = new RecyclerViewOnScrollListener(mLayoutManager);

            mRecyclerView.AddOnScrollListener(onScrollListener);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);


            mRecyclerView.SetItemClickListener((rv, position, view) =>
            {
                Context context = view.Context;
                Intent intent = new Intent(context, typeof(EnterPage));
                //intent.PutExtra("Nag³ówek 2", 1);

                context.StartActivity(intent);
            });

            // £adowanie nastêpnych newsów po dojechaniu na sam dó³ listy newsów
            onScrollListener.LoadMoreEvent += (object sender, EventArgs e) => {

                iloscnewsow += 5;  // zwiêksz iloœc newsów o 5 w póŸniejszym czasie ³adowanie do listy nowych obiektów
                
                //mAdapter.NotifyDataSetChanged();
                mAdapter = new RecyclerAdapter(iloscnewsow);
                mRecyclerView.SetAdapter(mAdapter);
                
            };

            ((AppCompatActivity)this.Activity).SupportActionBar.Title = "Wiadomoœci";

            return mRecyclerView;
        }

        public class RecyclerAdapter : RecyclerView.Adapter
        {

            public int liczba = 12;

            public RecyclerAdapter(int ilosc)
            {

                liczba = ilosc;
            }

            public class MyView : RecyclerView.ViewHolder
            {

                public TextView mName { get; set; }
                public MyView(View view) : base(view)
                {

                }
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.News, parent, false);

                TextView txtName = row.FindViewById<TextView>(Resource.Id.textViewNews3);


                MyView view = new MyView(row) { mName = txtName };
                return view;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                MyView view = holder as MyView;

                view.mName.Text = "15:" + Convert.ToString(10 + position);
            }

            public override int ItemCount
            {
                get { return liczba; }  //iloœæ newsów w przysz³oœci JakasListaNewsów.Count
            }

            public void Zmienilosc(int a)
            {
                liczba = a;

            }

        } 

    }
}