using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Widget;
using System;
using Xamarin.Forms;
using static Test.Constants;

namespace Test.Droid
{
    [Activity(Label = APP_NAME, Icon = "@drawable/icon", MainLauncher = true, Theme = "@style/AppToolbarTheme", NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait,
           ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal TrackerServiceConnection ServiceConnection;
        public bool IsStarting;
        public static MainActivity Instance { get; private set; }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Instance = this;
            Log.Info(TAG, $"OnCreate MainActivity Pid={Process.MyPid()}");
            Forms.Init(this, bundle);
            ServiceConnection = new TrackerServiceConnection();
            try
            {
                LoadApplication(new App());
            }
            catch (Exception ex) { Console.WriteLine($"{TAG} Erreur {ex.Source} et {ex.Message}"); }
        }
        protected override void OnStart()
        {
            base.OnStart();
            var serviceToStart = new Intent();
            serviceToStart.SetComponent(new ComponentName(PACKAGE_NAME, TRACKER_COMPONENT_NAME));
            BindService(serviceToStart, ServiceConnection, Bind.AutoCreate);
            IsStarting = true;
        }
        protected override void OnResume()
        {
            base.OnResume();
            try
            {
                MessagingCenter.Subscribe<ToTrackerMessage>(this, nameof(ToTrackerMessage), (message) => {
                    if (ServiceConnection.Messenger == null)
                    {
                        Toast.MakeText(this, "Service non connecté!", ToastLength.Short).Show();
                        return;
                    }
                    ServiceConnection.Messenger.Send(Message.Obtain(null, (int)message.MessageType));
                });
            } catch { }
        }
        protected override void OnPause()
        {
            MessagingCenter.Unsubscribe<string>(this, nameof(ToTrackerMessage));
            base.OnPause();
        }
        protected override void OnStop()
        {
            UnbindService(ServiceConnection);
            base.OnStop();
        }
        class AppPreferences : IAppPreferences
        {
            public void ForToast(string message, bool shortMsg = false) { Toast.MakeText(Instance, message, shortMsg ? ToastLength.Short : ToastLength.Long).Show(); }
        }
    }
}