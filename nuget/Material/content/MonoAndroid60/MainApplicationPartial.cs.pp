using System;
using Android.App;
using Android.OS;
using Android.Runtime;

namespace $rootnamespace$
{
	//You can specify additional application information in this attribute
    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          :base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            Material.Framework.Platform.Current.Context = activity;
            
            var data = activity.Intent?.Data?.ToString();
            if (data != null)
            {
                //necessary for custom uri scheme OAuth callbacks to function
                Material.Framework.Platform.Current.Protocol(new Uri(data));
            }
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            Material.Framework.Platform.Current.Context = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            Material.Framework.Platform.Current.Context = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}