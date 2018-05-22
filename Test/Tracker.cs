using Realms;
using Realms.Sync;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Test.Constants;

namespace Test
{
    public class Tracker : ITracker
    {
        static private Tracker _Tracker;
        private Tracker() { StartTrack(true); }
        static public Tracker Instance { get { return _Tracker ?? (_Tracker = new Tracker()); } }
        public IQueryable<TrackMessage> TrackMessage { get; set; }
        public void StatusTrack() { AddMsg("What else?"); }
        public Timer Tmr { get; set; }
        public int Counter { get; set; }
        public void StopTrack()
        {
            if (Tmr == null) return;
            Tmr.Dispose();
            Tmr = null;
            Counter = 0;
        }
        public async void StartTrack(bool isBoot = false)
        {
            try
            {
                if (Tmr != null) return;
                Tmr = new Timer(new TimerCallback(HeartBeat), this, Counter = 0, 5000);
                await StartAsync();
            }
            catch (Exception e) { Console.WriteLine($"{TAG} Erreur {nameof(StartTrack)} {e.GetType().FullName} {e.Message}"); }
        }
        private void HeartBeat(Object state)
        {
            var str = $"Counter { Counter++}";
            Console.WriteLine($"{TAG} {str}");
            //AddMsg(str);
        }
        public void AddMsg(string msg)
        {
            try
            {
                if (User.Current == null) return;
                using (var transaction = MyRealm.RealmContext.BeginWrite())
                {
                    MyRealm.RealmContext.Add(new TrackMessage { Id = Counter, Text = msg });
                    transaction.Commit();
                }
            }
            catch (Exception e) { Console.WriteLine($"{TAG} Erreur {e.GetType().FullName} {e.Message}"); }
        }
        public async Task StartAsync()
        {
            Realm RealmContext = null;
            try
            {
                if (User.AllLoggedIn.Count() > 1)
                    foreach (var _u in User.AllLoggedIn)
                        await _u.LogOutAsync();
                if (User.Current == null)
                    if (await MyRealm.Login(API_USERNAME, API_PASSWORD) == null)
                        return;
                RealmContext = MyRealm.RealmContext;
                using (var transaction = RealmContext.BeginWrite())
                {
                    RealmContext.RemoveAll<TrackMessage>();
                    RealmContext.Add(new TrackMessage { Id = Counter, Text = "Service ready" });
                    transaction.Commit();
                }
                TrackMessage = RealmContext.All<TrackMessage>();
            }
            catch (Exception e) { Console.WriteLine($"{TAG} Erreur {e.GetType().FullName} {e.Message}"); }
        }
    }
}