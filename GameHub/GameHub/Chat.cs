using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Refractored.Controls;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using GameHub.Fragments;
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
        private LinearLayoutManager mLayoutManager;
        private List<Message_class> message_list = new List<Message_class>();
        private List<Message_class> messageHistory_list = new List<Message_class>();
        private ReadWriteAppFile historia;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Chat);

            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolBar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            string friend = Intent.GetStringExtra("Name");
            SupportActionBar.Title = friend;

            ImageView imagev = FindViewById<ImageView>(Resource.Id.sendImage);
            //imagev.SetOnClickListener(SendClickListener);
            imagev.Click += delegate { SendClickListener(); };
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerview);
            mLayoutManager = new LinearLayoutManager(mRecyclerView.Context);
            mAdapter = new RecyclerAdapter(message_list);
            mLayoutManager.StackFromEnd = true;
            mLayoutManager.ReverseLayout = true;
            var onScrollListener = new RecyclerViewOnScrollListener(mLayoutManager);
            mRecyclerView.AddOnScrollListener(onScrollListener);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(mAdapter);

            historia = new ReadWriteAppFile(friend);
            try
            {
                var restoredOject = historia.RestoreObject();
                messageHistory_list = (List<Message_class>)restoredOject;
                int k = 0;

                for (int i = 0; i <= 10; i++)
                {
                    if (messageHistory_list.Count >= k + 1)
                    {
                        DateTime date = DateTime.Now;
                        Message_class mes = messageHistory_list[messageHistory_list.Count - (k + 1)];
                        message_list.Insert(0, mes);
                        k++;
                    }
                }
                mAdapter.NotifyDataSetChanged();
                mLayoutManager.ScrollToPosition(0);
            }
            catch { }

            onScrollListener.LoadMoreEvent += (object sender, EventArgs e) => {
                if (message_list[0].id > 0)
                {
                    Load_History(messageHistory_list);
                }
                else
                {
                    mRecyclerView.RemoveOnScrollListener(onScrollListener);
                }
            };

        }

        public void AngrySimon()
        {

            System.Threading.Thread.Sleep(500);
            List<string> an = new List<string>();
            var asset = Assets.Open("AngrySimon");
            StreamReader sraa = new StreamReader(asset);
            while (!sraa.EndOfStream)
            {
                an.Add(sraa.ReadLine());
            }
            Random rnd = new Random();
            int lo = rnd.Next(1, 72);
            string str = an[lo];
            str = str.Replace(" ", "+");
            byte[] bytes = Convert.FromBase64String(str);
            using (Aes angrySimon = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes("AngrySimon", new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                angrySimon.Key = pdb.GetBytes(32);
                angrySimon.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, angrySimon.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytes, 0, bytes.Length);
                        cs.Close();
                    }
                    str = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            DateTime date = DateTime.Now;
            Generator("AngrySimon", str, date, Resource.Drawable.deniedsmall);
        }

        public void SendClickListener()
        {
            EditText chatmessage = FindViewById<EditText>(Resource.Id.editTextM);
            if (chatmessage.Text != "" && chatmessage.Text != " ")
            {

                DateTime date = DateTime.Now;
                Generator("Nick", chatmessage.Text, date);
                chatmessage.Text = "";

                AngrySimon();


            }
        }


        public void Load_History(List<Message_class> history_list)
        {
            //System.Threading.Thread.Sleep(2000);
            for (int a = 0; a <= 5; a++)
            {
                if (message_list[0].id > 0)
                {
                    Message_class mes = history_list[message_list[0].id - 1];
                    message_list.Insert(0, mes);
                }
            }
            int numberOfChild = mRecyclerView.ChildCount;
            mAdapter.NotifyDataSetChanged();
            mLayoutManager.ScrollToPosition(numberOfChild);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public void Generator(string Nick, string message, DateTime date, int photo = Resource.Drawable.acceptsmall)
        {
            Message_class new_message = new Message_class(Nick, message, Convert.ToString(date.Day) + ":" + Convert.ToString(date.Month) + ":" + Convert.ToString(date.Year) + " " + Convert.ToString(date.Hour) + ":" + Convert.ToString(date.Minute));
            new_message.uri = photo;
            new_message.id = messageHistory_list.Count;
            message_list.Add(new_message);
            messageHistory_list.Add(new_message);
            mAdapter.NotifyDataSetChanged();
            mLayoutManager.ScrollToPosition(0);
            historia.SaveObject(messageHistory_list);
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
                view.mImage.SetImageResource(message_list[indexPosition].uri);
            }

            public override int ItemCount
            {
                get { return message_list.Count; }
            }

        }

        [Serializable()]
        public class Message_class
        {
            public int id = 0;
            public string nick;
            public string text;
            public string time;
            public int uri = Resource.Drawable.acceptsmall;

            public Message_class(string nick1, string text1, string time1)
            {
                nick = nick1;
                text = text1;
                time = time1;
            }

        }
    }
}