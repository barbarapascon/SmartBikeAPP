using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Geolocator;
using SmartBikeApp.Model;
using SmartBikeApp.Service;
using SmartBikeApp.ViewModel;
using Xamarin.Essentials;
//using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Markup;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace SmartBikeApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : MasterDetailPage
    {
        User usuarioLogado;
        public Main(User userLogged)
        {
            usuarioLogado = userLogged;
           
            InitializeComponent();
            
            PreparaTela();
            tapDestination_Tapped(new Object(), new EventArgs());          
        }

        private void NewRide_Tapped(object sender, EventArgs e)
        {
            DoAnimation(sender);
            ScannerAsync();            
            IsPresented = false;
        }

        private void tapPerfil_Tapped(object sender, EventArgs e)
        {
            DoAnimation(sender);
            Detail = new NavigationPage(new PerfilPage(usuarioLogado));
            IsPresented = false;

        }

        private void tapHistory_Tapped(object sender, EventArgs e)
        {
            DoAnimation(sender);
            Detail = new NavigationPage(new HistoricoPage());
            IsPresented = false;
        }

        private async void tapDestination_Tapped(object sender, EventArgs e)
        {
            DoAnimation(sender);
            bool emCorrida = await DataSeviceSmartBike.VerificaUsuarioCorrida(usuarioLogado);
            Detail = new NavigationPage(new FindBikesPage(MasterDetailHome, usuarioLogado, emCorrida));
            IsPresented = false;
        }
        private void tapAbout_Tapped(object sender, EventArgs e)
        {
            DoAnimation(sender);
            Detail = new NavigationPage(new AboutPage());
            IsPresented = false;
        }

        private async void tapLogout_Tapped(object sender, EventArgs e)
        {
            DoAnimation(sender);
            await Navigation.PopModalAsync();

            //Application.Current.MainPage = new LoginPage();
        }


        /// <summary>
        /// Animação para fazer com que os frames e os StackLayouts clicáveis, tenham animação parecida com a animação de botões.
        /// </summary>
        /// <param name="sender"></param>
        private async void DoAnimation(object sender)
        {
            const int _animationTime = 50;
            try
            {
                if (sender.GetType().Name == "StackLayout")
                {
                    var layout = (StackLayout)sender;
                    await layout.FadeTo(0.5, _animationTime);
                    await layout.FadeTo(1, _animationTime);
                }
                else if (sender.GetType().Name == "Frame")
                {
                    var layout = (Frame)sender;
                    await layout.FadeTo(0.5, _animationTime);
                    await layout.FadeTo(1, _animationTime);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Button animation error", ex.Message, "OK");
            }
        }

        private async void ScannerAsync()
        {
            ToolbarItem item = new ToolbarItem
            {
                Text = "Leitor de QR Code",
                IconImageSource = ImageSource.FromFile("qr_code.png"),
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };

            var scannerPage = new ZXingScannerPage();
            scannerPage.Title = "Leitor de QR Code";
            scannerPage.ToolbarItems.Add(item);

            await Detail.Navigation.PopToRootAsync();
            scannerPage.OnScanResult += (result) =>
            {                
                scannerPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Detail.Navigation.PopToRootAsync();
                    if (result.Text.Contains("urn:ngsi-ld:bicicleta:"))
                    {
                        var duration = TimeSpan.FromMilliseconds(200);
                        Vibration.Vibrate(duration);
                        Detail = new NavigationPage(new BicicletaInfoPage(MasterDetailHome, usuarioLogado, result.Text));
                    }
                    else
                    {
                        var duration = TimeSpan.FromMilliseconds(500);
                        Vibration.Vibrate(duration);
                        DisplayAlert("valor do QRcodeLidoInválido", result.Text, "OK");
                        return;
                    }
                });
            };
            await Detail.Navigation.PushAsync(scannerPage);
        }


        private void PreparaTela()
        {
            User_Name.Text = String.Format("{0} {1}",usuarioLogado.FirstName, usuarioLogado.LastName);
            //Lendo uma imagem base64 e atribuíndo à foto do usuário.
            byte[] Base64Stream = Convert.FromBase64String(usuarioLogado.ImgBase64);
            User_image.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));

        }
    }
}