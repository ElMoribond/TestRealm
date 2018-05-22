namespace Test
{
    public interface ITracker
    {
        void StopTrack();
        void StartTrack(bool isBoot = false);
        void StatusTrack();
    }
}