using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Refractored.Controls;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Content.PM;

namespace GameHub
{
    [Activity(Label = "Chat", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Chat : AppCompatActivity
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.Adapter mAdapter;
        private List<Message_class> message_list = new List<Message_class>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Chat);

            for (int a = 0; a < 1; a++)
            {
                Generator();
            }

            ImageView imagev = FindViewById<ImageView>(Resource.Id.sendImage);
            //imagev.SetOnClickListener(SendClickListener);
            imagev.Click += delegate { SendClickListener(); };
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerview);
            var mLayoutManager = new LinearLayoutManager(mRecyclerView.Context);
            mAdapter = new RecyclerAdapter(message_list);
            mLayoutManager.StackFromEnd = true;
            mLayoutManager.ReverseLayout = true;
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

        }

        public void SendClickListener()
        {
            Generator();
            mAdapter.NotifyDataSetChanged();
        }

        public void Generator()
        {
            Message_class new_message = new Message_class("franek" + Convert.ToString(message_list.Count), "co ty tutaj robisz", "Wczora z wieczora");
            message_list.Add(new_message);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }

        public class RecyclerAdapter : RecyclerView.Adapter
        {

            private List<Message_class> message_list = new List<Message_class>();

            public RecyclerAdapter(List<Message_class> list)
            {
                message_list = list;
            }

            public class MyView : RecyclerView.ViewHolder
            {
                public View mMainView { get; set; }
                public CircleImageView mImage { get; set; }
                public TextView mNick { get; set; }
                public TextView mText { get; set; }
                public TextView mTime { get; set; }
                public MyView(View view) : base(view)
                {
                    mMainView = view;
                }
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ChatMessage, parent, false);

                TextView txtNick = row.FindViewById<TextView>(Resource.Id.textViewMessage1);
                TextView txtText = row.FindViewById<TextView>(Resource.Id.textViewMessage2);
                TextView txtTime = row.FindViewById<TextView>(Resource.Id.textViewMessage3);
                CircleImageView cimage = row.FindViewById<CircleImageView>(Resource.Id.imageCustomerIcon);

                MyView view = new MyView(row) { mNick = txtNick, mText = txtText, mTime = txtTime, mImage = cimage };
                return view;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                MyView view = holder as MyView;

                int indexPosition = (message_list.Count - 1) - position;
                //iew.mMainView.Click += mMainView_Click;
                view.mNick.Text = message_list[indexPosition].nick;
                view.mText.Text = message_list[indexPosition].text;
                view.mTime.Text = message_list[indexPosition].time;
                view.mImage.SetImageResource(Resource.Drawable.acceptsmall);
            }

            public override int ItemCount
            {
                get { return message_list.Count; }
            }

        }

        public class Message_class
        {
            public string nick;
            public string text;
            public string time;
            public Message_class(string nick1, string text1, string time1)
            {
                nick = nick1;
                text = text1;
                time = time1;
            }

        }

    }
}