using SmartBikeApp.Model;
using SmartBikeApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartBikeApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void LbEsqueciSenha_Tapped(object sender, EventArgs e)
        {

        }

        private async void Logar_Clicked(object sender, EventArgs e)
        {
            try
            {
                AtivaIndicador();
                User usuario = await DataSeviceSmartBike.GetUserFromAPIAsync(Username.Text, Password.Text);               
                NavigationPage.SetHasNavigationBar(this, true);
                
                await Navigation.PushModalAsync(new Main(usuario));
                DesativaIndicador();
            }
            catch(Exception ex)
            {
                DesativaIndicador();
                await DisplayAlert("Usuário incorreto", "Usuário ou senha incorretos" + ex.Message, "OK");
            }
            
        }
        private void AtivaIndicador()
        {            
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            indicator.IsEnabled = true;
            Username.IsReadOnly = true;
            Password.IsReadOnly = true;
        }

        private void DesativaIndicador()
        {
            indicator.IsVisible = false;
            indicator.IsRunning = false;
            indicator.IsEnabled = false;
            Username.IsReadOnly = false;
            Password.IsReadOnly = false;
        }

    }
}