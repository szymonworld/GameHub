using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Json;
using Android.Support.Design.Widget;
using Android.Support.V7.App;

namespace GameHub.Fragments
{
    public class LoginIN : SupportFragment
    {
        private static string LoginDataUser;
        private ISharedPreferences pref;
        View view;
        EditText password;
        EditText email;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.LoginIN, container, false);

            pref = ((AppCompatActivity)this.Activity).GetSharedPreferences(LoginDataUser, FileCreationMode.Private);
            RestoreData();
            ProgressDialog dialog;
            Button login = view.FindViewById<Button>(Resource.Id.LoginIn_ButtonLogin);

            login.Click += async(object sender, EventArgs args) =>
            {
                bool internetConnection = await API.checkForInternetConnection();
                dialog = ProgressDialog.Show(view.Context, "", GetString(Resource.String.LoginLoading), true);
                if (internetConnection)
                {
                     email = view.FindViewById<EditText>(Resource.Id.input_Email_In);
                     password = view.FindViewById<EditText>(Resource.Id.input_Password_In);

                    bool authentificated = await API.authentification(email.Text, password.Text);

                    if (authentificated)
                    {
                              dialog.Dismiss();
                            SaveData();
                            Context context = view.Context;
                            Intent intent = new Intent(context, typeof(MainActivity));
                            context.StartActivity(intent);
                    }
                    else
                    {
                        dialog.Dismiss();
                        Snackbar snackbar1 = Snackbar.Make(view, GetString(Resource.String.InvalidLoginOrPassword), Snackbar.LengthShort);
                        snackbar1.Show();
                    }
                }
                else
                {
                    dialog.Dismiss();
                    Snackbar snackbar1 = Snackbar.Make(view, GetString(Resource.String.NoInternetConnection), Snackbar.LengthShort);
                    snackbar1.Show();
                }
            };


            return view;

        }

        private void SaveData()
        {
            ISharedPreferencesEditor PrefEdit = pref.Edit();
            PrefEdit.PutString("PrefEmailUser", email.Text.ToString());
            PrefEdit.PutString("PrefPasswordUser", password.Text.ToString());
            PrefEdit.Apply(); 
        }
        private void RestoreData()
        {
            email = view.FindViewById<EditText>(Resource.Id.input_Email_In);
            password = view.FindViewById<EditText>(Resource.Id.input_Password_In);
            email.Text = pref.GetString("PrefEmailUser", "");
            password.Text = pref.GetString("PrefPasswordUser", "");
            
        }
    }
}