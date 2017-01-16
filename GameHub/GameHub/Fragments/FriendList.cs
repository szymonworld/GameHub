using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Graphics;
using GameHub.Fragments;
using Android.Util;
using Android.Content.Res;
using Android.Support.Design.Widget;
using DesignLibrary.Helpers;
using static Android.Content.ClipData;

namespace GameHub
{
    [Activity(Label = "FriendList")]
    public class FriendList : SupportFragment
    {

        private List<string> list = new List<string>();
        private RecyclerView.Adapter mAdapter;
        private RecyclerView mRecyclerView;
        

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view2 = inflater.Inflate(Resource.Layout.FriendList, container, false);

            SupportToolbar toolBar = view2.FindViewById<SupportToolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)this.Activity).SetSupportActionBar(toolBar);
            ((AppCompatActivity)this.Activity).SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            ((AppCompatActivity)this.Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            ((AppCompatActivity)this.Activity).SupportActionBar.Title = GetString(Resource.String.Friends_Title);


            mRecyclerView = view2.FindViewById<RecyclerView>(Resource.Id.recyclerview);

            int uri2 = Resource.Drawable.Icon2;
            GetRandomSubList(Lists.Friends.NickStrings, 2);  // dodanie 30 obiektów do listy 

            var mLayoutManager = new LinearLayoutManager(mRecyclerView.Context);
            var onScrollListener = new RecyclerViewOnScrollListener(mLayoutManager);
            mRecyclerView.AddOnScrollListener(onScrollListener);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mAdapter = new SimpleStringRecyclerViewAdapter(mRecyclerView.Context, list, uri2, view2);
            
            mRecyclerView.SetAdapter(mAdapter);

            FloatingActionButton myFab = (FloatingActionButton)view2.FindViewById(Resource.Id.fab);
            myFab.Click += delegate
            {
                GetRandomSubList(Lists.Friends.NickStrings, 1);
                if (uri2 == Resource.Drawable.Icon2)
                {
                    uri2 = Resource.Drawable.acceptsmall;
                }
                else
                {
                    uri2 = Resource.Drawable.Icon2;
                }
                mAdapter = new SimpleStringRecyclerViewAdapter(mRecyclerView.Context, list, uri2, view2);
                mRecyclerView.SetAdapter(mAdapter);
                mAdapter.NotifyDataSetChanged();

                FragmentTransaction transaction = ((AppCompatActivity)this.Activity).FragmentManager.BeginTransaction();
                AddFriendDialogC inviteFriend = new AddFriendDialogC();
                inviteFriend.Show(transaction, "dialog");
            };

            

            mRecyclerView.SetItemClickListener((rv, position, view) =>
            {
                TextView mTxtView = view.FindViewById<TextView>(Resource.Id.text1);
                Context context = view2.Context;
                Intent intent = new Intent(context, typeof(Profile));
                intent.PutExtra("Name", mTxtView.Text);
                context.StartActivity(intent);

            });
            

            // Dodanie nowych obiektów do listy po dojechaniu na dó³
            onScrollListener.LoadMoreEvent += (object sender, EventArgs e) => {

                //GetRandomSubList(Lists.Friends.NickStrings, 5); // generowanie i dodanie noeych obiektów

                //mAdapter.NotifyDataSetChanged();
            };

            return view2;

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