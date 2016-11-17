using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.Widget;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GameHub
{
    class RecyclerAdapter : RecyclerView.Adapter
    {
        public RecyclerAdapter()
        {
        }

        public class MyView : RecyclerView.ViewHolder
        {
            public MyView(View view) : base(view)
            {

            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.News, parent, false);
            MyView view = new MyView(row) { };
            return view;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView view = holder as MyView;
        }

        public override int ItemCount
        {
            get { return 2; }
        }
    }
}