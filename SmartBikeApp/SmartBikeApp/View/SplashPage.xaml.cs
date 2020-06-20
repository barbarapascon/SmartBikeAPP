using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartBikeApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {
        Image smartSplashImage;
        public SplashPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            var sub = new AbsoluteLayout();
            smartSplashImage = new Image
            {
                Source = "SplashScreenImage.jpg",
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,

            };
            AbsoluteLayout.SetLayoutFlags(smartSplashImage, AbsoluteLayoutFlags.PositionProportional);

            sub.Children.Add(smartSplashImage);
            this.BackgroundColor = Color.White; // Color.FromHex("#A258C7");
            this.Content = sub;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await smartSplashImage.ScaleTo(1, 2000);
            await smartSplashImage.ScaleTo(0.9, 1500,Easing.Linear);
            await smartSplashImage.ScaleTo(150, 1200, Easing.Linear);
                        
            Application.Current.MainPage = new LoginPage();
        }

    }
}