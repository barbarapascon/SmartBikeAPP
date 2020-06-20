using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartBikeApp;
using SmartBikeApp.View;

namespace SmartBikeApp
{
    
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new SplashPage());
                        
            //MainPage = new SplashPage();
            //MainPage = new LoginPage();
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
