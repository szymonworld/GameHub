using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Widget;
using Android.Support.V7.App;

namespace GameHub.Fragments
{
    public class Quickgame : SupportFragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.Adapter mAdapter;
        private List<Wydarzenie> lista_wydarzen = new List<Wydarzenie>();
        private int naprzemiennieSzybkaITurniej = 0;
        private List<string> list = new List<string>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view2 = inflater.Inflate(Resource.Layout.Quickgame, container, false);

            SupportToolbar toolBar = view2.FindViewById<SupportToolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)this.Activity).SetSupportActionBar(toolBar);
            ((AppCompatActivity)this.Activity).SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            ((AppCompatActivity)this.Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            ((AppCompatActivity)this.Activity).SupportActionBar.Title = GetString(Resource.String.Quickgame_Title);


            mRecyclerView = view2.FindViewById<RecyclerView>(Resource.Id.recyclerview);




            return view2;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            GetRandomSubList(Lists.Friends.NickStrings, 2);
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    GetRandomSubList(Lists.Friends.NickStrings, 2);
                    ((AppCompatActivity)this.Activity).Finish();
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

        public class Wydarzenie
        {
            public int ift = 0;
            public string etitle;
            public string efteam;
            public string esteam;
            public string edate;
            public string egame;
            public Wydarzenie(int ifTournament, string title, string team1, string team2, string date, string game)
            {
                ift = ifTournament;
                etitle = title;
                efteam = team1;
                esteam = team2;
                edate = date;
                egame = game;
                //Czas(godzina, minuta);
            }

            private void Czas(int godzina, int minuta)
            {
                // wydarzenie_czas = Convert.ToString(godzina) + ":" + Convert.ToString(minuta);
            }
        }
    }
}