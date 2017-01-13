using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.Widget;
using Android.Views;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Support.V7.App;

namespace GameHub.Fragments
{
    class Events : SupportFragment
    {

        private RecyclerView mRecyclerView;
        private RecyclerView.Adapter mAdapter;
        private List<Wydarzenie> lista_wydarzen = new List<Wydarzenie>();
        private int naprzemiennieSzybkaITurniej = 0;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            for (int a = 0; a < 15; a++)
            {
                Generuj_wydarzenie();
            }
            mRecyclerView = inflater.Inflate(Resource.Layout.Friends, container, false) as RecyclerView;
            var mLayoutManager = new LinearLayoutManager(mRecyclerView.Context);
            mAdapter = new RecyclerAdapter(lista_wydarzen);

            var onScrollListener = new RecyclerViewOnScrollListener(mLayoutManager);
            mRecyclerView.AddOnScrollListener(onScrollListener);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

            // £adowanie nastêpnych wydarzeñ po dojechaniu na sam dó³
            onScrollListener.LoadMoreEvent += (object sender, EventArgs e) =>
            {

                for (int a = 0; a < 5; a++)
                {
                    Generuj_wydarzenie();
                }
                //mAdapter.NotifyDataSetChanged();
                mAdapter = new RecyclerAdapter(lista_wydarzen);
                mRecyclerView.SetAdapter(mAdapter);
            };

            ((AppCompatActivity)this.Activity).SupportActionBar.Title = "Wydarzenia";

            return mRecyclerView;
        }

        public void Generuj_wydarzenie()
        {
            //Random rnd = new Random();
            //int random = rnd.Next(20);
            // Wydarzenie nowe_wydarzenie = new Wydarzenie(random, "text ---- Text", "Wydarzenie", 20, 15);
            if (naprzemiennieSzybkaITurniej == 0)
            {
                Wydarzenie nowe_wydarzenie = new Wydarzenie(naprzemiennieSzybkaITurniej, "ATH CUP", "Informatycy", "Budowlanka", "18.05 22:15", "My Little Pony");
                naprzemiennieSzybkaITurniej = 1;
                lista_wydarzen.Add(nowe_wydarzenie);
            }
            else
            {
                Wydarzenie nowe_wydarzenie = new Wydarzenie(naprzemiennieSzybkaITurniej, "Turniej Gfinity 2016 PRO League", "Virtus.pro", "Fnatic", "28.05 20:15", "Counter-Strike: Global Offensive");
                naprzemiennieSzybkaITurniej = 0;
                lista_wydarzen.Add(nowe_wydarzenie);
            }
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
                if (lista_wydarzen[indexPosition].ift == 0)
                {
                    view.mLayout.SetBackgroundResource(Resource.Drawable.Tournament);
                    //view.mLayout.SetBackgroundColor(Android.Graphics.Color.ParseColor("#2196f3"));
                }
                else
                {
                    view.mLayout.SetBackgroundResource(Resource.Drawable.QuickGame);
                    //view.mLayout.SetBackgroundColor(Android.Graphics.Color.ParseColor("#ef6c00"));
                }

            }

            public override int ItemCount
            {
                get { return lista_wydarzen.Count; }  //iloœæ newsów w przysz³oœci JakasListaNewsów.Count
            }
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