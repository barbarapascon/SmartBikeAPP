using SmartBikeApp.Model;
using SmartBikeApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
                var internet = Connectivity.NetworkAccess;

                if (internet == NetworkAccess.Internet)
                {
                    AtivaIndicador();
                    User usuario = await DataSeviceSmartBike.GetUserFromAPIAsync(Username.Text, Password.Text);
                    NavigationPage.SetHasNavigationBar(this, true);

                    await Navigation.PushModalAsync(new Main(usuario));
                    DesativaIndicador();
                }
                else
                {
                    await DisplayAlert("Falha de rede.", "Você não possui acesso à Internet. Verifique o acesso e tente novamente.", "OK");
                }
            }
            catch (Exception ex)
            {
                DesativaIndicador();
                await DisplayAlert("Falha na autenticação.", "Usuário ou senha incorretos", "OK");
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