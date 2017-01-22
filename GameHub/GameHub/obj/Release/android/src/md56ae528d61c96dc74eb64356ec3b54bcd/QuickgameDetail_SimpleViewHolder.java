package md56ae528d61c96dc74eb64356ec3b54bcd;


public class QuickgameDetail_SimpleViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_toString:()Ljava/lang/String;:GetToStringHandler\n" +
			"";
		mono.android.Runtime.register ("GameHub.QuickgameDetail+SimpleViewHolder, GameHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", QuickgameDetail_SimpleViewHolder.class, __md_methods);
	}


	public QuickgameDetail_SimpleViewHolder (android.view.View p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == QuickgameDetail_SimpleViewHolder.class)
			mono.android.TypeManager.Activate ("GameHub.QuickgameDetail+SimpleViewHolder, GameHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Views.View, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public java.lang.String toString ()
	{
		return n_toString ();
	}

	private native java.lang.String n_toString ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
