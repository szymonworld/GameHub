using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.Widget;
using Android.Views;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Support.V7.App;
using DesignLibrary.Helpers;

namespace GameHub.Fragments
{
    public class NotificationPanel : SupportFragment
    {

        private RecyclerView mRecyclerView;
        private RecyclerView.Adapter mAdapter;
        private List<Wydarzenie> lista_wydarzen = new List<Wydarzenie>();

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

            mRecyclerView.SetItemClickListener((rv, position, view) =>
            {
                Dialog("Powiadomienia", "Zaakceptowaæ?");
            });

            // £adowanie nastêpnych wydarzeñ po dojechaniu na sam dó³
            onScrollListener.LoadMoreEvent += (object sender, EventArgs e) => {

                for (int a = 0; a < 5; a++)
                {
                    Generuj_wydarzenie();
                }
                //mAdapter.NotifyDataSetChanged();
                mAdapter = new RecyclerAdapter(lista_wydarzen);
                mRecyclerView.SetAdapter(mAdapter);
            };

            return mRecyclerView;
        }

        void Dialog(string title, string msg)
        {
            var builder = new Android.Support.V7.App.AlertDialog.Builder((AppCompatActivity)this.Activity, Resource.Style.DarkThemeDialog);
            builder.SetTitle(title);
            builder.SetMessage(msg);
            builder.SetPositiveButton(GetString(Resource.String.Dialog_Positive), (EventHandler<DialogClickEventArgs>)null);
            builder.SetNegativeButton(GetString(Resource.String.Dialog_Negative), (EventHandler<DialogClickEventArgs>)null);
            var dialog = builder.Create();
            dialog.Show();
            var PositiveButton = dialog.GetButton((int)DialogButtonType.Positive);
            PositiveButton.Click += delegate
            {
                dialog.Cancel();
            };
            var NegativeButton = dialog.GetButton((int)DialogButtonType.Positive);
            NegativeButton.Click += delegate
            {
                dialog.Cancel();
            };
        }

        public void Generuj_wydarzenie()
        {
                Wydarzenie nowe_wydarzenie = new Wydarzenie("U¿ytkownik @AdamPaluch zaprosi³ Ciê do gry.", GetString(Resource.String.not_titleTournament), 20, 15);
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
                View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.NotificationPanel, parent, false);

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
                view.mLayout.SetBackgroundResource(Resource.Drawable.Add);


            }

            public override int ItemCount
            {
                get { return lista_wydarzen.Count; }  //iloœæ newsów w przysz³oœci JakasListaNewsów.Count
            }
        }

        public class Wydarzenie
        {
            public int czy_turniej = 0;
            public string wydarzenie_text;
            public string wydarzenie_tytul;
            public string wydarzenie_czas;
            public Wydarzenie(string Text, string tytul, int godzina, int minuta)
            {
                //czy_turniej = czyturniej;
                wydarzenie_text = Text;
                wydarzenie_tytul = tytul;
                Czas(godzina, minuta);
            }

            private void Czas(int godzina, int minuta)
            {
                wydarzenie_czas = Convert.ToString(godzina) + ":" + Convert.ToString(minuta);
            }
        }

        class NotificationClickPanel : Android.App.DialogFragment
        {
            private Button mBtnAccept;
            private Button mBtnDenied;

            public event EventHandler<OnSignUpEventArgs> mOnSignUpComplete;
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.NotificationClickPanel, container, false);

                mBtnAccept = view.FindViewById<Button>(Resource.Id.buttonAccept);
                mBtnDenied = view.FindViewById<Button>(Resource.Id.buttonDenied);


                mBtnAccept.Click += MBtnAccept_Click;
                mBtnDenied.Click += MBtnDenied_Click;

                return view;
            }

            private void MBtnDenied_Click(object sender, EventArgs e)
            {
                this.Dismiss();
            }

            private void MBtnAccept_Click(object sender, EventArgs e)
            {
                this.Dismiss();
            }

            private void mBtnSignUp_Click(object sender, EventArgs e)
            {
                //mOnSignUpComplete.Invoke(this, new OnSignUpEventArgs(mTxtFirstName.Text, mTxtEmail.Text, mTxtPassword.Text));
                //this.Dismiss();
            }

            public override void OnActivityCreated(Bundle savedInstanceState)
            {
                Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

                base.OnActivityCreated(savedInstanceState);

            }
        }

    }
}