using System;

namespace Test
{
    public class ToTrackerMessage
    {
        public enum TypeMessage { Ignore = 880, Start = 882, Stop = 883, NeedResponse = 885 };
        public readonly TypeMessage MessageType = TypeMessage.Ignore;
        public readonly int Tempo;
        public ToTrackerMessage(bool startOrStop) { MessageType = startOrStop ? TypeMessage.Start : TypeMessage.Stop; }
        public ToTrackerMessage(TypeMessage messageType)
        {
            MessageType = messageType;
        }
        static public TypeMessage FromInt(int messageType)
        {
            foreach (var mType in Enum.GetValues(typeof(TypeMessage)))
                if (messageType == (int)mType)
                    return (TypeMessage)mType;
            return TypeMessage.Ignore;
        }
    }
}