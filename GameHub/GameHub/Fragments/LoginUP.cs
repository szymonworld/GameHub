using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Threading;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using GameHub.Fragments;
using Android.Graphics;

namespace GameHub.Fragments
{
    public class LoginUP : SupportFragment
    {
        private static string LoginDataUser;
        private ISharedPreferences pref;
        View view;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.LoginUP, container, false);

            pref = ((AppCompatActivity)this.Activity).GetSharedPreferences(LoginDataUser, FileCreationMode.Private);

            Button login = view.FindViewById<Button>(Resource.Id.LoginUp_ButtonRegister);


            TextView gotologin = view.FindViewById<TextView>(Resource.Id.gotologin);
            gotologin.Click += delegate {

                var newFragment = new LoginIN();
                var ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.LoginFrame, newFragment);
                ft.AddToBackStack(null);
                ft.Commit();
            };

            login.Click += async (object sender, EventArgs args) =>
            {
                bool internetConnection = await API.checkForInternetConnection();
                ProgressDialog dialog;
                if (internetConnection)
                {
                    EditText email = view.FindViewById<EditText>(Resource.Id.input_Email);
                    dialog = new ProgressDialog(this.Activity, Resource.Style.AppCompatAlertDialogStyle);
                    dialog.SetMessage(GetString(Resource.String.LoginLoading));
                    dialog.Show();

                    bool accountExist = await API.isAccountExist(email.Text);

                    if (!accountExist)
                    {
                        EditText password = view.FindViewById<EditText>(Resource.Id.input_Password);
                        EditText passwordConfirm = view.FindViewById<EditText>(Resource.Id.input_PasswordConfirm);

                        if (password.Text.Count() > 0)
                        {

                            if (password.Text == passwordConfirm.Text)
                            {
                                EditText nickname = view.FindViewById<EditText>(Resource.Id.input_Nick);
                                if (nickname.Text.Count() > 0)
                                {
                                    Account account = new Account() { Email = email.Text, Password = password.Text, Login = nickname.Text };

                                    bool accountCreated = await API.createAccount(account);
                                    if (accountCreated)
                                    {
                                        dialog.Dismiss();
                                        SaveData();
                                        Context context = view.Context;
                                        Intent intent = new Intent(context, typeof(MainActivity));
                                        context.StartActivity(intent);
                                    }
                                    else
                                    {
                                        ShowSnack(view, GetString(Resource.String.CannotCreateAccount));
                                    }
                                }
                                else
                                {
                                    ShowSnack(view, GetString(Resource.String.TypeNickname));
                                }

                            }
                            else
                            {
                                ShowSnack(view, GetString(Resource.String.PasswordsNotEqual));
                            }

                        }
                        else
                        {
                            ShowSnack(view, GetString(Resource.String.PasswordTooShort));
                        }

                    }
                    else
                    {
                        ShowSnack(view, GetString(Resource.String.AccountExist));
                    }

                    dialog.Dismiss();
                }
                else
                {
                    ShowSnack(view, GetString(Resource.String.NoInternetConnection));
                }

            };

            return view;
        }

        private void MBtnSignIn_Click(object sender, EventArgs e)
        {
            Context context = view.Context;
            Intent intent = new Intent(context, typeof(MainActivity));
            context.StartActivity(intent);
        }

        private void SignupDialog_mOnSignUpComplete(object sender, OnSignUpEventArgs e)
        {
            // mProgressBar.Visibility = Android.Views.ViewStates.Visible;
            Thread t1 = new Thread(ActLikeARequest);
            t1.Start();
        }

        private void ActLikeARequest()
        {
            Thread.Sleep(2000);

            // RunOnUiThread(() => { mProgressBar.Visibility = Android.Views.ViewStates.Invisible; });

        }
        private void SaveData()
        {
            EditText password = view.FindViewById<EditText>(Resource.Id.input_Password);
            EditText email = view.FindViewById<EditText>(Resource.Id.input_Email);

            ISharedPreferencesEditor PrefEdit = pref.Edit();
            PrefEdit.PutString("PrefEmailUser", email.Text.ToString());
            PrefEdit.PutString("PrefPasswordUser", password.Text.ToString());
            PrefEdit.Apply();
        }
        private void ShowSnack(View view, string msg)
        {
            Snackbar snackbar1 = Snackbar.Make(view, msg, Snackbar.LengthShort);
            View snackBarView = snackbar1.View;
            snackBarView.SetBackgroundColor(Color.ParseColor("#333d59"));
            snackbar1.Show();
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