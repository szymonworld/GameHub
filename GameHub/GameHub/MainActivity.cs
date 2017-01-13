using Android.App;
using Android.Views;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Widget;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using Android.Support.Design.Widget;
using Android.Content;
using Android.Support.V4.View;
using Android.Support.V4.App;
using System.Collections.Generic;
using Java.Lang;
using GameHub.Fragments;
using Android.Views.Animations;
using Android.Content.PM;
using System;
using Refractored.Controls;
using System.Threading.Tasks;

namespace GameHub
{
    [Activity(Label = "GameHub", ScreenOrientation = ScreenOrientation.Portrait) ]
    public class MainActivity : AppCompatActivity
    {
        private static string LoginDataUser;
        private ISharedPreferences pref;
        private DrawerLayout mDrawerLayout;
        private Android.Support.V7.App.ActionBarDrawerToggle drawerToggle;
        private SupportToolbar toolbar;
        private NavigationView navigationView;
        public Account currentAccount;

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            Intent intent;
            SupportFragment newFragment = new Hub();
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.flContent, newFragment, "Hub");
            trans.Commit();

            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            View navheader = navigationView.GetHeaderView(0);
            CircleImageView iv = (CircleImageView)navheader.FindViewById(Resource.Id.imageCustomerIcon);
            iv.Click += delegate
            {
                intent = new Intent(this, typeof(Profile));
                OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                this.StartActivity(intent);
            };

            await LoadAccount();

            navigationView.NavigationItemSelected += (sender, e) =>
            {
                SupportFragment frag;
                e.MenuItem.SetChecked(true);
                switch (e.MenuItem.ItemId)
                {
                    
                    case (Resource.Id.nav_hub):

                        frag = (SupportFragment)SupportFragmentManager.FindFragmentByTag("Hub");
                        newFragment = new Hub();
                        trans = SupportFragmentManager.BeginTransaction();
                        trans.Replace(Resource.Id.flContent, newFragment, "Hub");
                        if (frag == null)
                        {
                            trans.AddToBackStack("Hub");
                        }
                        trans.Commit();
                        OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                        break;

                    case (Resource.Id.nav_tournament):
                         frag = (SupportFragment)SupportFragmentManager.FindFragmentByTag("Turnieje");
                        newFragment = new Tournament(null,null, null, null, null);
                        trans = SupportFragmentManager.BeginTransaction();
                        trans.Replace(Resource.Id.flContent, newFragment, "Turnieje");
                        if (frag == null)
                        {
                            trans.AddToBackStack("Turnieje");
                        }
                        trans.Commit();
                        OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                        break;

                    case (Resource.Id.nav_quickgame):
                        frag = (SupportFragment)SupportFragmentManager.FindFragmentByTag("Szybka gra");
                        newFragment = new Quickgame();
                        trans = SupportFragmentManager.BeginTransaction();
                        trans.Replace(Resource.Id.flContent, newFragment, "Szybka gra");
                        if (frag == null)
                        {
                            trans.AddToBackStack("Szybka gra");
                        }
                        trans.Commit();
                        OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                        break;

                    case (Resource.Id.nav_findplayers):
                        frag = (SupportFragment)SupportFragmentManager.FindFragmentByTag("Znajdz gracza");
                        newFragment = new Findplayers();
                        trans = SupportFragmentManager.BeginTransaction();
                        trans.Replace(Resource.Id.flContent, newFragment, "Znajdz gracza");
                        if (frag == null)
                        {
                            trans.AddToBackStack("Znajdz gracza");
                        }
                        trans.Commit();
                        OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                        break;
                    case (Resource.Id.nav_friends):
                        frag = (SupportFragment)SupportFragmentManager.FindFragmentByTag("Znajomi");
                        newFragment = new FriendList();
                        trans = SupportFragmentManager.BeginTransaction();
                        trans.Replace(Resource.Id.flContent, newFragment, "Znajomi");
                        if (frag == null)
                        {
                            trans.AddToBackStack("Znajomi");
                        }
                        trans.Commit();
                        OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                        break;

                    case (Resource.Id.nav_profile):
                        intent = new Intent(this, typeof(Profile));
                        OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                        this.StartActivity(intent);
                        

                        break;

                    case (Resource.Id.nav_addQuickgame):
                        intent = new Intent(this, typeof(AddQuickgame));
                        OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                        this.StartActivity(intent);

                        break;

                    default:
                        break;
                }

                mDrawerLayout.CloseDrawers();





            };
        }

        //public void SendClickListener()
        //{
        //    intent = new Intent(this, typeof(Profile));
        //    OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
        //    this.StartActivity(intent);
        // }
        // }
        private async Task LoadAccount()
        {
            pref = ((AppCompatActivity)this).GetSharedPreferences(LoginDataUser, FileCreationMode.Private);
            currentAccount = await API.getAccountByEmail(pref.GetString("PrefEmailUser", ""));
            TextView profilename = (TextView)navigationView.GetHeaderView(0).FindViewById(Resource.Id.textCustomer2);
            profilename.Text = currentAccount.Login;

        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbarMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent intent;
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    mDrawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
                    break;
                case Resource.Id.menut_ustawienia:
                    intent = new Intent(this, typeof(Settings));
                    OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                    this.StartActivity(intent);
                    break;
                case Resource.Id.MainToolbarLogout:
                    pref = GetSharedPreferences(LoginDataUser, FileCreationMode.Private);
                    ISharedPreferencesEditor PrefEdit = pref.Edit();
                    PrefEdit.Clear();
                    PrefEdit.Apply();
                    intent = new Intent(this, typeof(LoginSystem));
                    OverridePendingTransition(Resource.Animation.animRight, Resource.Animation.animRight2);
                    this.StartActivity(intent);
                    Finish();
                    break;
                case Resource.Id.menut_search:
                    Dialog();
                    break;
            }

            //Toast.MakeText(this, "Zaznaczone: " + item.TitleFormatted,
            //    ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);

        }
        void Dialog()
        {


            var builder = new Android.Support.V7.App.AlertDialog.Builder(this);
            var dialogview = LayoutInflater.Inflate(Resource.Layout.SearchDialog, null);
            builder.SetView(dialogview);

            Spinner dropdown = dialogview.FindViewById<Spinner>(Resource.Id.spinner1);
            string[] platform = new string[] { "Turniej", "Szybka gra" };
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, platform);
            dropdown.Adapter = adapter;

            Spinner dropdown2 = dialogview.FindViewById<Spinner>(Resource.Id.spinner2);
            string[] platform2 = new string[] { "Steam", "Origin", "Uplay", "Battle.net", "XBOX LIVE", "PSN", "Skype", "LOL", "Discord" };
            ArrayAdapter<string> adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, platform2);
            dropdown2.Adapter = adapter2;

            var AutoComGameList = new string[] { "CS:GO", "", "LOL", "DOTA", "HS", "Watch Dogs 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "Call of Duty Modern Warfare 2", "GTA 5", "Tomb Raider: Rise of the" };
            ArrayAdapter AutoComGameListAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, AutoComGameList);
            var TextGamelist = dialogview.FindViewById<AutoCompleteTextView>(Resource.Id.autoCompleteTextView1);
            TextGamelist.Adapter = AutoComGameListAdapter;

            dialogview.FindViewById(Resource.Id.spinner1);
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


    }


}

