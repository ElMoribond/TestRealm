using Realms;
using Realms.Sync;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Test
{
    public class TrackMessage : RealmObject
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public TrackMessage() { }
        public override string ToString() { return Text; }
    }
    static public class MyRealm
    {
        static private SyncConfiguration GetConfig(User user = null) => new SyncConfiguration(user ?? User.Current, Constants.RealmUri);
        static private Realm _RealmContext;
        static public Realm RealmContext
        {
            get
            {
                try
                {
                    return _RealmContext ?? (_RealmContext = Realm.GetInstance(GetConfig()));
                }
                catch (Exception e) { Console.WriteLine($"{Constants.TAG} Erreur {nameof(RealmContext)} {e.GetType().FullName} {e.Message}"); return null; }
            }
        }
        static public async Task<User> Login(string email, string password, bool create = false)
        {
            try
            {
                User user;
                foreach (var _user in User.AllLoggedIn)
                    await _user.LogOutAsync();
                _RealmContext = Realm.GetInstance(GetConfig(user = await User.LoginAsync(Credentials.UsernamePassword(email, password, create), Constants.AuthServerUri)));
                _RealmContext.Write(() => _RealmContext.Add(new TrackMessage { Id = _RealmContext.All<TrackMessage>().Count() + 1, Text = "Login" }));
                return User.Current;
            }
            catch (AuthenticationException) { Console.WriteLine($"{Constants.TAG} Erreur {(create ? "Already exists" : "Unknown Username and Password combination")}"); }
            catch (SocketException sockEx) { Console.WriteLine($"{Constants.TAG} Erreur Network error: {sockEx}"); }
            catch (WebException webEx) { Console.WriteLine($"{Constants.TAG} Erreur {nameof(Login)} {(webEx.Status == WebExceptionStatus.ConnectFailure ? $"Unable to connect to Server" : "Error trying to login")} {webEx.Message}"); }
            catch (Exception e) { Console.WriteLine($"{Constants.TAG} Erreur {nameof(Login)} {(User.Current == null ? "Error trying to login" : "Credentials accepted but then failed to open Realm")} {e.GetType().FullName} {e.Message}"); }
            return null;
        }
    }
}