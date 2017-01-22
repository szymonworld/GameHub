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
using Android.Webkit;
using Android.Graphics;

namespace GameHub
{
    [Activity(Label = "Browser")]
    public class Browser : AppCompatActivity
    {
        ProgressBar progressbar;
        WebClient mWebClient;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Browser);
            //probar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            mWebClient = new WebClient();
            progressbar = FindViewById<ProgressBar>(Resource.Id.progbar);
            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            progressbar.Visibility = ViewStates.Invisible;

            mWebClient.onProgressChanged += (int state) =>
            {
                if (state == 0)
                {
                    progressbar.Visibility = ViewStates.Invisible;
                }
                else
                {
                    progressbar.Visibility = ViewStates.Visible;
                }
            };


            SetSupportActionBar(toolBar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.Title = GetString(Resource.String.Browser);
            string Link = Intent.GetStringExtra("BrowserLink");
            WebView Browser = FindViewById<WebView>(Resource.Id.webView1);
            Browser.Settings.JavaScriptEnabled = true;
            Browser.SetWebViewClient(mWebClient);
            Browser.LoadUrl(Link);



        }
        public class WebClient : WebViewClient
        {
            public delegate void ToggleProgressBar(int state);
            public ToggleProgressBar onProgressChanged;

            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                view.LoadUrl(url);
                return true;
            }

            public override void OnPageStarted(WebView view, string url, Bitmap favicon)
            {
                if (onProgressChanged != null)
                {
                    onProgressChanged.Invoke(1);

                }
                base.OnPageStarted(view, url, favicon);
            }
            public override void OnPageFinished(WebView view, string url)
            {
                if (onProgressChanged != null)
                {
                    onProgressChanged.Invoke(0);
                }
                base.OnPageFinished(view, url);
            }

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
    }

}