using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;
using static Test.Constants;

namespace Test.Droid
{
	public class TrackerServiceConnection : Java.Lang.Object, IServiceConnection
	{
		public Messenger Messenger { get; private set; }
		public void OnServiceConnected(ComponentName name, IBinder service)
		{
            Log.Debug(TAG, $"{nameof(OnServiceConnected)} {name.ClassName}");
            try
            {
                if ((Messenger = new Messenger(service)) != null)
                    MainActivity.Instance.IsStarting = false;
            }
            catch { Log.Debug(TAG, $"Erreur {nameof(OnServiceConnected)}"); }
            Toast.MakeText(MainActivity.Instance, "OnServiceConnected", ToastLength.Short).Show();
        }
        public void OnServiceDisconnected(ComponentName name)
		{
            Toast.MakeText(MainActivity.Instance, nameof(OnServiceConnected), ToastLength.Short).Show();
            Log.Debug(TAG, $"{nameof(OnServiceDisconnected)}");
			Messenger = null;
        }
    }
}