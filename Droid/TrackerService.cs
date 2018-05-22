using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;
using static Test.Constants;

namespace Test.Droid
{
    [Service(Name = TRACKER_COMPONENT_NAME, Exported = true, Permission = TRACKER_PERMISSION, Process = TRACKER_PROCESS)]
    public class TrackerService : Service, ITracker
    {
        static public TrackerService Instance { get; private set; }
        private ITracker tracker;
        private Messenger messenger;
        public TrackerService() { Instance = this; }
        public void StopTrack() { tracker?.StopTrack(); }
        public void StartTrack(bool isBoot = false) { tracker?.StartTrack(false); }
        public void StatusTrack() { tracker?.StatusTrack(); }
        public override void OnCreate()
		{
			base.OnCreate();
            messenger = new Messenger(new TrackerRequestHandler(this));
            tracker = Tracker.Instance;
            Log.Info(TAG, "TrackerService process id:" + Process.MyPid());
        }
        public override IBinder OnBind(Intent intent)
        {
            tracker?.StatusTrack();
            return messenger.Binder;
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var str = $"StartCommandResult {startId}";
            Log.Debug(TAG, str);
            tracker?.StatusTrack();
            Toast.MakeText(this, str, ToastLength.Long).Show();
            return StartCommandResult.RedeliverIntent;
        }
        public void OnResume()
        {
            Log.Info(TAG, $"{nameof(OnResume)} de {nameof(TrackerService)} ");
        }
        public override void OnDestroy()
        {
            messenger.Dispose();
            tracker = null;
            Log.Info(TAG, $"{nameof(OnDestroy)} de {nameof(TrackerService)} ");
            base.OnDestroy();
        }
    }
}