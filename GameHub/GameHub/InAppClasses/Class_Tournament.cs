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
    public class Class_Tournament : SupportFragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.Adapter mAdapter;
        private List<Wydarzenie> lista_wydarzen = new List<Wydarzenie>();
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
            public string FTeam { get; set; }
            public string STeam { get; set; }
            public string Date { get; set; }
            public string Game { get; set; }
        }


        public Class_Tournament(string title, string t1, string t2, string date, string game)
        {

            c.Title = title;
            c.FTeam = t1;
            c.STeam = t2;
            c.Date = date;
            c.Game = game;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.TournamentCard, container, false);
            Class_Tournament aaa = new Class_Tournament("KURWA MA� turniej jebany w morde jechany", "Virtus.pro", "Fnatic", "28.05 20:15", "Counter-Strike: Global Offensive");

            TextView EventTitle = view.FindViewById<TextView>(Resource.Id.EventTitle);
            TextView EventFirstTeam = view.FindViewById<TextView>(Resource.Id.EventFirstTeam);
            TextView EventSecondaryTeam = view.FindViewById<TextView>(Resource.Id.EventSecondaryTeam);
            TextView EventDate = view.FindViewById<TextView>(Resource.Id.EventDate);
            TextView EventGame = view.FindViewById<TextView>(Resource.Id.EventGame);

            EventTitle.Text = c.Title;
            EventFirstTeam.Text = c.FTeam;
            EventSecondaryTeam.Text = c.STeam;
            EventDate.Text = c.Date;
            EventGame.Text = c.Game;


            return view;
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
        public void Generuj_wydarzenie()
        {
            Wydarzenie nowe_wydarzenie = new Wydarzenie("Turniej Gfinity 2016 PRO League", "Virtus.pro", "Fnatic", "28.05 20:15", "Counter-Strike: Global Offensive");
            lista_wydarzen.Add(nowe_wydarzenie);

        }

        public class RecyclerAdapter : RecyclerView.Adapter
        {
            private List<Wydarzenie> lista_wydarzen = new List<Wydarzenie>();

            public RecyclerAdapter(List<Wydarzenie> list)
            {
                lista_wydarzen = list;
            }

            public class MyView : RecyclerView.ViewHolder
            {
                public View mMainView { get; set; }
                public TextView ETitle { get; set; }
                public TextView EFTeam { get; set; }
                public TextView ESTeam { get; set; }
                public TextView EDate { get; set; }
                public TextView EGame { get; set; }
                public LinearLayout mLayout { get; set; }
                public MyView(View view) : base(view)
                {
                    mMainView = view;
                }
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.TournamentCard, parent, false);

                TextView EventTitle = row.FindViewById<TextView>(Resource.Id.EventTitle);
                TextView EventFirstTeam = row.FindViewById<TextView>(Resource.Id.EventFirstTeam);
                TextView EventSecondaryTeam = row.FindViewById<TextView>(Resource.Id.EventSecondaryTeam);
                TextView EventDate = row.FindViewById<TextView>(Resource.Id.EventDate);
                TextView EventGame = row.FindViewById<TextView>(Resource.Id.EventGame);



                Refractored.Controls.CircleImageView platform = row.FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.EventPlatformIcon);
                LinearLayout colorLi = row.FindViewById<LinearLayout>(Resource.Id.colorLayoutWydarzenia);
                //li.SetBackgroundColor(Android.Graphics.Color.Red);

                MyView view = new MyView(row) { ETitle = EventTitle, EFTeam = EventFirstTeam, ESTeam = EventSecondaryTeam, EDate = EventDate, EGame = EventGame, mLayout = colorLi };
                return view;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                MyView view = holder as MyView;

                int indexPosition = (lista_wydarzen.Count - 1) - position;
                //iew.mMainView.Click += mMainView_Click;
                view.ETitle.Text = lista_wydarzen[indexPosition].etitle;
                view.EFTeam.Text = lista_wydarzen[indexPosition].efteam;
                view.ESTeam.Text = lista_wydarzen[indexPosition].esteam;
                view.EDate.Text = lista_wydarzen[indexPosition].edate;
                view.EGame.Text = lista_wydarzen[indexPosition].egame;
                //view.mTime.Text = Convert.ToString(lista_wydarzen[indexPosition].czy_turniej);
                view.mLayout.SetBackgroundResource(Resource.Drawable.Tournament);



            }

            public override int ItemCount
            {
                get { return lista_wydarzen.Count; }  //ilo�� news�w w przysz�o�ci JakasListaNews�w.Count
            }
        }



        public class Wydarzenie
        {
            public string etitle;
            public string efteam;
            public string esteam;
            public string edate;
            public string egame;
            public Wydarzenie(string title, string team1, string team2, string date, string game)
            {
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