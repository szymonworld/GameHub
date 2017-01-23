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
using Android.Support.V7.App;
using Android.Widget;

namespace GameHub.Fragments
{
    class Events : SupportFragment
    {

        private RecyclerView mRecyclerView;
        private RecyclerView.Adapter mAdapter;
        private List<Event_class> lista_wydarzen = new List<Event_class>();
        private int naprzemiennieSzybkaITurniej = 0;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            for (int a = 0; a < 15; a++)
            {
                Generuj_Event_class();
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
                    Generuj_Event_class();
                }
                //mAdapter.NotifyDataSetChanged();
                mAdapter = new RecyclerAdapter(lista_wydarzen);
                mRecyclerView.SetAdapter(mAdapter);
            };

            ((AppCompatActivity)this.Activity).SupportActionBar.Title = "Wydarzenia";

            return mRecyclerView;
        }

        public void Generuj_Event_class()
        {
            //Random rnd = new Random();
            //int random = rnd.Next(20);
            // Event_class nowe_Event_class = new Event_class(random, "text ---- Text", "Event_class", 20, 15);
            if (naprzemiennieSzybkaITurniej == 0)
            {
                Event_class nowe_Event_class = new Event_class("Turniej Gfinity 2016 PRO League", "Virtus.pro", "Fnatic", "28.05 20:15", "Counter-Strike: Global Offensive");
                lista_wydarzen.Add(nowe_Event_class);
                naprzemiennieSzybkaITurniej = 1;
            }
            else
            {
                Event_class nowe_Event_class = new Event_class("ATH CUP", "18.05 22:15", "My Little Pony");
                lista_wydarzen.Add(nowe_Event_class);
                naprzemiennieSzybkaITurniej = 0;

            }
        }

        public class RecyclerAdapter : RecyclerView.Adapter
        {
            private List<Event_class> lista_wydarzen = new List<Event_class>();

            public RecyclerAdapter(List<Event_class> list)
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
                public TextView Description { get; set; }
                public LinearLayout mLayout { get; set; }
                public MyView(View view) : base(view)
                {
                    mMainView = view;
                }
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                if (viewType == 0)
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
                else
                {
                    View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.QuickgameCard, parent, false);

                    row.Click += (sender, e) =>
                    {
                        Context context = row.Context;
                        Intent intent = new Intent(context, typeof(QuickgameDetail));
                        context.StartActivity(intent);
                    };

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
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                int itemType = GetItemViewType(position);

                if (itemType == 0)
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
                else
                {

                    MyView view = holder as MyView;

                    int indexPosition = (lista_wydarzen.Count - 1) - position;
                    //iew.mMainView.Click += mMainView_Click;
                    view.ETitle.Text = lista_wydarzen[indexPosition].etitle;
                    //view.Description.Text = lista_wydarzen[indexPosition].desc;
                    view.EDate.Text = lista_wydarzen[indexPosition].edate;
                    view.EGame.Text = lista_wydarzen[indexPosition].egame;
                    //view.mTime.Text = Convert.ToString(lista_wydarzen[indexPosition].czy_turniej);
                    view.mLayout.SetBackgroundResource(Resource.Drawable.QuickGame);
                }

            }

            public override int GetItemViewType(int position)
            {
                if (lista_wydarzen[position].type == 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }

            public override int ItemCount
            {
                get { return lista_wydarzen.Count; }  //iloœæ newsów w przysz³oœci JakasListaNewsów.Count
            }
        }

    }
}