using System;

namespace Test
{
    static public class Constants
    {
        public const string API_SERVER = "ros_server";
        public const string API_USERNAME = "username";
        public const string API_PASSWORD = "password";
        public static Uri AuthServerUri => new Uri($"https://{API_SERVER}:9443");
        public static Uri RealmUri => new Uri($"realm://{API_SERVER}:9080/~/default");

        public const string APP_NAME = "Test";
        public const string TAG = "App" + APP_NAME;
        public const string PACKAGE_NAME = "fr.test.aps";
        public const string TRACKER_COMPONENT_NAME = PACKAGE_NAME + ".TrackerService";
        public const string TRACKER_PERMISSION = PACKAGE_NAME + ".REQUEST_TRACKER";
        public const string TRACKER_PROCESS = PACKAGE_NAME + ".tracker";
    }
}