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
using DesignLibrary.Helpers;

namespace GameHub.Fragments
{
    public class Quickgame : SupportFragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.Adapter mAdapter;
        private List<Event_class> list = new List<Event_class>();
        private List<Event> eventList;
        private string LoginDataUser;
        private ISharedPreferences pref;
        View view;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
            pref = ((AppCompatActivity)this.Activity).GetSharedPreferences(LoginDataUser, FileCreationMode.Private);
        }
        public override void OnResume()
        {
            base.OnResume();
            LoadEvents();
        }
        public async void LoadEvents()
        {
            bool internetConnection = await API.checkForInternetConnection();
            if (internetConnection)
            {
                eventList = await API.getEvents(pref.GetString("PrefEmailUser", ""), pref.GetString("PrefPasswordUser", ""));
                list.Clear();
                foreach (Event ev in eventList)
                {
                    Event_class e = new Event_class(ev.EventName, ev.Datee + " " + ev.Hourr, ev.GameID);
                    //Event_class e = new Event_class(ev.EventName, "", "");
                    list.Add(e);
                }

                mAdapter = new RecyclerAdapter(list);
                mRecyclerView.SetAdapter(mAdapter);
            }
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            ((AppCompatActivity)this.Activity).MenuInflater.Inflate(Resource.Menu.Search_Menu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.Quickgame, container, false);
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerview);
            //mRecyclerView = inflater.Inflate(Resource.Layout.Friends, container, false) as RecyclerView;
            var mLayoutManager = new LinearLayoutManager(((AppCompatActivity)this.Activity));
            mAdapter = new RecyclerAdapter(list);

            var onScrollListener = new RecyclerViewOnScrollListener(mLayoutManager);
            mRecyclerView.AddOnScrollListener(onScrollListener);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

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

            mRecyclerView.SetItemClickListener((rv, position, view) =>
            {
                Intent intent = new Intent(((AppCompatActivity)this.Activity), typeof(QuickgameDetail));
                ((AppCompatActivity)this.Activity).StartActivity(intent);

            });


            return view;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    ((AppCompatActivity)this.Activity).Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
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
                //view.Description.Text = lista_wydarzen[indexPosition].desc;
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

        
    }
}