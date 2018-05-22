using Realms;
using Realms.Sync;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Test.Constants;

namespace Test
{
    public class ServicePage : ContentPage
    {
        public IQueryable<TrackMessage> TrackMessage { get; set; }
        private ListView listView = new ListView();
        private Button CreateButton = new Button { Text = "Create user" };
        private Button StartButton = new Button { Text = "Start service" };
        private Button StopButton = new Button { Text = "Stop" };
        public ServicePage()
        {
            var buttonGrid = new Grid();
            buttonGrid.Children.Add(StopButton, 0, 0);
            buttonGrid.Children.Add(StartButton, 1, 0);
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Star) });
            grid.Children.Add(CreateButton, 0, 0);
            grid.Children.Add(buttonGrid, 0, 1);
            grid.Children.Add(listView, 0, 2);
            Content = grid;
        }
        public async Task InitListView()
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
                    RealmContext.Add(new TrackMessage { Id = 0, Text = "Please wait, service starting" });
                    transaction.Commit();
                }
                listView.ItemsSource = TrackMessage = RealmContext.All<TrackMessage>();
            }
            catch (Exception e) { Console.WriteLine($"{Constants.TAG} Erreur {e.GetType().FullName} {e.Message}"); }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitListView();
            CreateButton.Clicked += Clicked;
            StartButton.Clicked += Clicked;
            StopButton.Clicked += Clicked;
        }
        protected override void OnDisappearing()
        {
            CreateButton.Clicked -= Clicked;
            StartButton.Clicked -= Clicked;
            StopButton.Clicked -= Clicked;
            base.OnDisappearing();
        }
        private async void Clicked(object sender, EventArgs e)
        {
            if (sender == StartButton || sender == StopButton)
                MessagingCenter.Send(new ToTrackerMessage(sender == StartButton), nameof(ToTrackerMessage));
            else if (sender == CreateButton)
                if (await MyRealm.Login(API_USERNAME, API_PASSWORD, true) != null)
                    await InitListView();
        }
    }
}