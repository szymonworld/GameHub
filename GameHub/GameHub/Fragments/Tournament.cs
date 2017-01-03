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

namespace GameHub.Fragments
{
    public class Tournament : SupportFragment
    {

        private RecyclerView mRecyclerView;
        private RecyclerView.Adapter mAdapter;
        private List<Wydarzenie> lista_wydarzen = new List<Wydarzenie>();
        View view;
        private List<string> list = new List<string>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.Tournament, container, false);
            for (int a = 0; a < 15; a++)
            {
                Generuj_wydarzenie();
            }
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerview);
            //mRecyclerView = inflater.Inflate(Resource.Layout.Friends, container, false) as RecyclerView;
            var mLayoutManager = new LinearLayoutManager(((AppCompatActivity)this.Activity));
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

            SupportToolbar toolBar = view.FindViewById<SupportToolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)this.Activity).SetSupportActionBar(toolBar);
            ((AppCompatActivity)this.Activity).SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            ((AppCompatActivity)this.Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            ((AppCompatActivity)this.Activity).SupportActionBar.Title = GetString(Resource.String.Tournament_Title);
            return view;
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
                View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.Events, parent, false);

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
                get { return lista_wydarzen.Count; }  //iloœæ newsów w przysz³oœci JakasListaNewsów.Count
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