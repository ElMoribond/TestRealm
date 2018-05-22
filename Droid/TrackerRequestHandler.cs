using System;
using Android.OS;

namespace Test.Droid
{
    public class TrackerRequestHandler : Handler
    {
        WeakReference<TrackerService> serviceRef;
        public TrackerRequestHandler(TrackerService service) { serviceRef = new WeakReference<TrackerService>(service); }
        TrackerService Service { get { return serviceRef.TryGetTarget(out TrackerService service) ? service : null; } }
        public override void HandleMessage(Message message)
        {
            var messageType = ToTrackerMessage.FromInt(message.What);
            if (messageType == ToTrackerMessage.TypeMessage.Stop)
                Service?.StopTrack();
            else if (messageType == ToTrackerMessage.TypeMessage.NeedResponse)
                Service?.StatusTrack();
            else if (messageType == ToTrackerMessage.TypeMessage.Start)
                Service?.StartTrack();
        }
    }
}