using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartBikeApp;

namespace SmartBikeApp
{
    
    public partial class App : Application
    {
        /*public static double ScreenHeight;
        public static double ScreenWidth;*/
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
