package md512a55c3b44ca3dc23ae108e7e4e4928b;


public class Class_Tournament_RecyclerAdapter_MyView
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("GameHub.InAppClasses.Class_Tournament+RecyclerAdapter+MyView, GameHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Class_Tournament_RecyclerAdapter_MyView.class, __md_methods);
	}


	public Class_Tournament_RecyclerAdapter_MyView (android.view.View p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == Class_Tournament_RecyclerAdapter_MyView.class)
			mono.android.TypeManager.Activate ("GameHub.InAppClasses.Class_Tournament+RecyclerAdapter+MyView, GameHub, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Views.View, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}

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
