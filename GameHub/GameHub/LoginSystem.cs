using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using Android.Content.PM;

namespace GameHub
{
    [Activity(Label = "GameHub", MainLauncher = false, Icon = "@drawable/icon4", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginSystem : Activity
    {
        private ProgressBar mProgressBar;
        private Button mBtnSignUp;
        private Button mBtnSignIn;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.Login);

            mBtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            mBtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);

            mBtnSignUp.Click += (object sender, EventArgs args) =>
            {
                
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                dialog_signup signupDialog = new dialog_signup();
                signupDialog.Show(transaction, "dialog fregment");

                signupDialog.mOnSignUpComplete += SignupDialog_mOnSignUpComplete;
            };

            mBtnSignIn.Click += MBtnSignIn_Click;


        }

        private void MBtnSignIn_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent);
        }

        private void SignupDialog_mOnSignUpComplete(object sender, OnSignUpEventArgs e)
        {
            mProgressBar.Visibility = Android.Views.ViewStates.Visible;
            Thread t1 = new Thread(ActLikeARequest);
            t1.Start();



        }

        private void ActLikeARequest()
        {
            Thread.Sleep(2000);

            RunOnUiThread(() => { mProgressBar.Visibility = Android.Views.ViewStates.Invisible; });

        }
    }


    public class OnSignUpEventArgs : EventArgs
    {
        private string mFirstName;
        private string mEmail;
        private string mPassword;

        public string FirstName
        {
            get { return mFirstName; }
            set { mFirstName = value; }
        }
        public string Email
        {
            get { return mEmail; }
            set { mEmail = value; }
        }
        public string Password
        {
            get { return mPassword; }
            set { mPassword = value; }
        }

        public OnSignUpEventArgs(string firstname, string email, string password) : base()
        {
            FirstName = firstname;
            Email = email;
            Password = password;

        }

    }
    class dialog_signup : DialogFragment
    {
        private EditText mTxtFirstName;
        private EditText mTxtEmail;
        private EditText mTxtPassword;
        private Button mBtnSignUp;

        public event EventHandler<OnSignUpEventArgs> mOnSignUpComplete;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.dialog_signup, container, false);

            mTxtFirstName = view.FindViewById<EditText>(Resource.Id.txtFirstName);
            mTxtEmail = view.FindViewById<EditText>(Resource.Id.txtEmail);
            mTxtPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            mBtnSignUp = view.FindViewById<Button>(Resource.Id.btnDialogEmail);

            mBtnSignUp.Click += mBtnSignUp_Click;

            return view;
        }

        private void mBtnSignUp_Click(object sender, EventArgs e)
        {
            mOnSignUpComplete.Invoke(this, new OnSignUpEventArgs(mTxtFirstName.Text, mTxtEmail.Text, mTxtPassword.Text));
            this.Dismiss();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            base.OnActivityCreated(savedInstanceState);
            
        }
    }
}