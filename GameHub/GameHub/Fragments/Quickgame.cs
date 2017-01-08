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
using Android.Support.Design.Widget;

namespace GameHub.Fragments
{
    public class Quickgame : SupportFragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.Adapter mAdapter;
        private List<Wydarzenie> lista_wydarzen = new List<Wydarzenie>();
        private List<string> list = new List<string>();
        View view;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.Quickgame, container, false);

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
            ((AppCompatActivity)this.Activity).SupportActionBar.Title = GetString(Resource.String.Quickgame_Title);


            FloatingActionButton myFab = (FloatingActionButton)view.FindViewById(Resource.Id.fab);
            myFab.Click += delegate
            {
                Context context = view.Context;
                Intent intent = new Intent(context, typeof(AddQuickgame));
                context.StartActivity(intent);
            };


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
            Wydarzenie nowe_wydarzenie = new Wydarzenie("ATH CUP", "To jest opis wydarzenia który powinien byæ krótki.", "18.05 22:15", "My Little Pony");
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
                public TextView Description { get; set; }

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
                View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.QuickgameCard, parent, false);

                TextView EventTitle = row.FindViewById<TextView>(Resource.Id.EventTitle);
                TextView Description = row.FindViewById<TextView>(Resource.Id.desc);
                TextView EventDate = row.FindViewById<TextView>(Resource.Id.EventDate);
                TextView EventGame = row.FindViewById<TextView>(Resource.Id.EventGame);



                Refractored.Controls.CircleImageView platform = row.FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.EventPlatformIcon);
                LinearLayout colorLi = row.FindViewById<LinearLayout>(Resource.Id.colorLayoutWydarzenia);
                //li.SetBackgroundColor(Android.Graphics.Color.Red);

                MyView view = new MyView(row) { ETitle = EventTitle, Description = Description, EDate = EventDate, EGame = EventGame, mLayout = colorLi };
                return view;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                MyView view = holder as MyView;

                int indexPosition = (lista_wydarzen.Count - 1) - position;
                //iew.mMainView.Click += mMainView_Click;
                view.ETitle.Text = lista_wydarzen[indexPosition].etitle;
                view.Description.Text = lista_wydarzen[indexPosition].desc;
                view.EDate.Text = lista_wydarzen[indexPosition].edate;
                view.EGame.Text = lista_wydarzen[indexPosition].egame;
                //view.mTime.Text = Convert.ToString(lista_wydarzen[indexPosition].czy_turniej);
                view.mLayout.SetBackgroundResource(Resource.Drawable.QuickGame);



            }

            public override int ItemCount
            {
                get { return lista_wydarzen.Count; }  //iloœæ newsów w przysz³oœci JakasListaNewsów.Count
            }
        }

        public class Wydarzenie
        {
            public string etitle;
            public string desc;
            public string edate;
            public string egame;
            public Wydarzenie(string title, string description, string date, string game)
            {
                etitle = title;
                desc = description;
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