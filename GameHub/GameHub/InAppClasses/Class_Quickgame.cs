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

namespace GameHub.InAppClasses
{
    public class Class_Quickgame : SupportFragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.Adapter mAdapter;
        View view;
        private List<string> list = new List<string>();
        Card c = new Card();
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public class Card
        {
            public string Title { get; set; }
            public string Freespace { get; set; }
            public string Date { get; set; }
            public string Game { get; set; }
        }


        public Class_Quickgame(string title, string freespace, string date, string game)
        {

            c.Title = title;
            c.Freespace = freespace;
            c.Date = date;
            c.Game = game;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.QuickgameCard, container, false);
            TextView EventTitle = view.FindViewById<TextView>(Resource.Id.EventTitle);
            TextView FreespaceNumber = view.FindViewById<TextView>(Resource.Id.FreespaceNumber);
            TextView EventDate = view.FindViewById<TextView>(Resource.Id.EventDate);
            TextView EventGame = view.FindViewById<TextView>(Resource.Id.EventGame);

            EventTitle.Text = c.Title;
            FreespaceNumber.Text = c.Freespace;
            EventDate.Text = c.Date;
            EventGame.Text = c.Game;


            return view;
        }
    }
}