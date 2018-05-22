using Xamarin.Forms;

namespace Test
{
    public partial class App : Application
    {
        public App()
        {
            MainPage = new NavigationPage(new ServicePage());
        }
    }
}