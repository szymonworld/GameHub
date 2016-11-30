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
    class Wydarzenia : SupportFragment
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

            // �adowanie nast�pnych wydarze� po dojechaniu na sam d�
            onScrollListener.LoadMoreEvent += (object sender, EventArgs e) => {

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
                Wydarzenie nowe_wydarzenie = new Wydarzenie(naprzemiennieSzybkaITurniej, "text ---- Text", "Wydarzenie", 20, 15);
                naprzemiennieSzybkaITurniej = 1;
                lista_wydarzen.Add(nowe_wydarzenie);
            }
            else
            {
                Wydarzenie nowe_wydarzenie = new Wydarzenie(naprzemiennieSzybkaITurniej, "text ---- Text", "Wydarzenie", 20, 15);
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
                public TextView mTitle { get; set; }
                public TextView mText { get; set; }
                public TextView mTime { get; set; }
                public LinearLayout mLayout { get; set; }
                public MyView(View view) : base(view)
                {
                    mMainView = view;
                }
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.Wydarzenia, parent, false);

                TextView txtTitle = row.FindViewById<TextView>(Resource.Id.textViewWydarzenia1);
                TextView txtText = row.FindViewById<TextView>(Resource.Id.textViewWydarzenia2);
                TextView txtTime = row.FindViewById<TextView>(Resource.Id.textViewWydarzenia3);
                LinearLayout colorLi = row.FindViewById<LinearLayout>(Resource.Id.colorLayoutWydarzenia);
                //li.SetBackgroundColor(Android.Graphics.Color.Red);

                MyView view = new MyView(row) { mTitle = txtTitle, mText = txtText, mTime = txtTime, mLayout = colorLi };
                return view;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                MyView view = holder as MyView;

                int indexPosition = (lista_wydarzen.Count - 1) - position;
                //iew.mMainView.Click += mMainView_Click;
                view.mTitle.Text = lista_wydarzen[indexPosition].wydarzenie_tytul;
                view.mText.Text = lista_wydarzen[indexPosition].wydarzenie_text;
                view.mTime.Text = lista_wydarzen[indexPosition].wydarzenie_czas;
                //view.mTime.Text = Convert.ToString(lista_wydarzen[indexPosition].czy_turniej);
                if (lista_wydarzen[indexPosition].czy_turniej == 0)
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
                get { return lista_wydarzen.Count; }  //ilo�� news�w w przysz�o�ci JakasListaNews�w.Count
            }
        }

        public class Wydarzenie
        {
            public int czy_turniej = 0;
            public string wydarzenie_text;
            public string wydarzenie_tytul;
            public string wydarzenie_czas;
            public Wydarzenie(int czyturniej, string Text, string tytul, int godzina, int minuta)
            {
                czy_turniej = czyturniej;
                wydarzenie_text = Text;
                wydarzenie_tytul = tytul;
                Czas(godzina, minuta);
            }

            private void Czas(int godzina, int minuta)
            {
                wydarzenie_czas = Convert.ToString(godzina) + ":" + Convert.ToString(minuta);
            }
        }

    }
}