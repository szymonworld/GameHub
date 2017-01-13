using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Content.PM;
using Android.Support.V7.Widget;
using Android.Graphics;
using GameHub.Fragments;
using Android.Util;
using Android.Content.Res;
using Android.Support.Design.Widget;
using DesignLibrary.Helpers;

namespace GameHub
{
    [Activity(Label = "QuickgameDetail", ScreenOrientation = ScreenOrientation.Portrait)]
    public class QuickgameDetail : AppCompatActivity
    {
        private List<string> list = new List<string>();
        private RecyclerView.Adapter mAdapter;
        private RecyclerView mRecyclerView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.QuickgameDetail);
            SupportToolbar toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.Title = "Szybka gra";

            //test
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerview);

            int uri2 = Resource.Drawable.Icon2;
            GetRandomSubList(Lists.Friends.NickStrings, 2);  // dodanie 30 obiektów do listy 

            var mLayoutManager = new LinearLayoutManager(mRecyclerView.Context);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mAdapter = new SimpleStringRecyclerViewAdapter(mRecyclerView.Context, list, uri2);

            mRecyclerView.SetAdapter(mAdapter);

            mRecyclerView.SetItemClickListener((rv, position, view) =>
            {

                TextView mTxtView = view.FindViewById<TextView>(Resource.Id.text1);
                Context context = this;
                Intent intent = new Intent(context, typeof(Chat));
                intent.PutExtra("Name", mTxtView.Text);
                context.StartActivity(intent);
            });

        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.QuickgameDetail_Join:
                    Dialog("Do³¹czyæ", "Chcesz do³¹czyæ do wydarzenie ?");
                    break;
                case Resource.Id.QuickgameDetail_Report:
                    Dialog("Zg³oœ", "Jesteœ pewien, ¿e chcesz zg³osiæ wydarzenie ?");
                    break;
                case Resource.Id.QuickgameDetail_Delete:
                    Dialog("Usuñ", "Na pewno chcesz usun¹æ wydarzenie ?");
                    break;
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.QuickgameDetail_Menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        void Dialog(string title, string msg)
        {
            var builder = new Android.Support.V7.App.AlertDialog.Builder(this);
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
        public class SimpleViewHolder : RecyclerView.ViewHolder
        {
            public string mBoundString;
            public readonly View mView;
            public readonly ImageView mImageView;
            public readonly TextView mTxtView;

            public SimpleViewHolder(View view, Action<int> listener) : base(view)
            {
                mView = view;
                mImageView = view.FindViewById<ImageView>(Resource.Id.avatar);
                mTxtView = view.FindViewById<TextView>(Resource.Id.text1);
                view.Click += (sender, e) => listener(Position);
            }

            public override string ToString()
            {
                return base.ToString() + " '" + mTxtView.Text;
            }
        }


        public class SimpleStringRecyclerViewAdapter : RecyclerView.Adapter
        {
            private readonly TypedValue mTypedValue = new TypedValue();
            private int mBackground;
            private List<string> mValues;
            public event EventHandler<int> ItemClick;
            private Dictionary<int, int> mCalculatedSizes;
            private int uri;

            public SimpleStringRecyclerViewAdapter(Context context, List<string> items, int resource)
            {
                //context.Theme.ResolveAttribute(Resource.Attribute.colorPrimary, mTypedValue, true);
                mBackground = mTypedValue.ResourceId;
                mValues = items;
                uri = resource;
                //mResource = res;

                mCalculatedSizes = new Dictionary<int, int>();
            }

            public override int ItemCount
            {
                get
                {
                    return mValues.Count;
                }
            }
            void OnClick(int position)
            {
                if (ItemClick != null)
                    ItemClick(this, position);
            }

            public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                var simpleHolder = holder as SimpleViewHolder;
                simpleHolder.mBoundString = mValues[position];
                simpleHolder.mTxtView.Text = mValues[position];

                int drawableID = Lists.Friends.RandomNick;
                BitmapFactory.Options options = new BitmapFactory.Options();

                if (mCalculatedSizes.ContainsKey(drawableID))
                {
                    options.InSampleSize = mCalculatedSizes[drawableID];
                }

                else
                {
                    options.InJustDecodeBounds = true;



                    options.InSampleSize = Lists.Friends.CalculateInSampleSize(options, 100, 100);
                    options.InJustDecodeBounds = false;

                    mCalculatedSizes.Add(drawableID, options.InSampleSize);
                }


                //var bitMap = await BitmapFactory.DecodeResourceAsync(mResource, drawableID, options);

                simpleHolder.mImageView.SetImageResource(uri);
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.List_Item, parent, false);
                //view.SetBackgroundResource(mBackground);


                return new SimpleViewHolder(view, OnClick);
            }
        }



        class AddFriendDialogC : DialogFragment
        {
            private Button mSendInvitation;
            private EditText mText;

            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.AddFriendDialog, container, false);

                mSendInvitation = view.FindViewById<Button>(Resource.Id.buttonAccept);
                mText = view.FindViewById<EditText>(Resource.Id.input_Email);

                mSendInvitation.Click += MBtnSend_Click;

                return view;
            }

            private void MBtnSend_Click(object sender, EventArgs e)
            {
                string mail = mSendInvitation.Text;
                try
                {
                    var addr = new System.Net.Mail.MailAddress(mSendInvitation.Text);
                    //mail = addr.Address;
                }
                catch
                {
                    mail = "err";
                }


                if (mail == mSendInvitation.Text)
                {
                    this.Dismiss();
                }
                else
                {
                    //Context.Resources.GetString(Resource.String);
                    mSendInvitation.SetError("Poda³êœ niepoprawny email!", null);
                }

            }

            public override void OnActivityCreated(Bundle savedInstanceState)
            {
                Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

                base.OnActivityCreated(savedInstanceState);

            }
        }
    }
}