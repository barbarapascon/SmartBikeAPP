using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Geolocator;
using SmartBikeApp.Interfaces;
using SmartBikeApp.Model;
using SmartBikeApp.Service;
using SmartBikeApp.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Markup;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace SmartBikeApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindBikesPage : ContentPage
    {
        readonly MasterDetailPage masterDt;
        Position posicaoAtual;
        bool bikeLocked;
        User usuarioLogado;

        public FindBikesPage(MasterDetailPage masterDetail, User userLogged, bool usuarioEmCorrida)
        {
            usuarioLogado = userLogged;
            bikeLocked = usuarioEmCorrida;
            masterDt = masterDetail;
            GetLocation();
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            this.BindingContext = new Tempo();

            SetPins();

            if (bikeLocked)
            {
                ImageButton.Source = @"lock.png";
            }
        }

        private async void GetLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(100));
                if (CrossGeolocator.Current.IsGeolocationEnabled)
                {
                    Tempo previsaoDoTempo = await DataService.GetPrevisaoDoTempoGeoLocation(position.Latitude.ToString(), position.Longitude.ToString());
                    //PreparaMapa(position.Latitude, position.Latitude, previsaoDoTempo.Title);                    
                    this.BindingContext = previsaoDoTempo;
                    posicaoAtual = new Position(position.Latitude, position.Longitude);
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(posicaoAtual, Distance.FromMeters(100));
                    map.MoveToRegion(mapSpan);

                }
                else
                {
                    await DisplayAlert("Message", "GPS not enabled", "OK");
                }

            }
            catch (Exception err)
            {
                await DisplayAlert("Erro no serviço de previsão do tempo", err.Message, "OK");
            }
        }


        private async void SetPins()
        {
            List<Coordenada> CoordenadasBikes = await DataSeviceSmartBike.RetornaBicicletasDisponiveis(usuarioLogado);
            SettingPins p = new SettingPins(CoordenadasBikes);

            foreach (Pin pino in p.pins)
            {
                pino.InfoWindowClicked += async (s, args) =>
                {
                    Pin pinoClicado = ((Pin)s);

                    double distance = Math.Round(Distance.BetweenPositions(posicaoAtual, pinoClicado.Position).Meters, 0);

                    string pinName = pinoClicado.Label;
                    await DisplayAlert($"Distancia até o bicicletário {pinName.ToLower()}.", String.Format("Você está a {0} metros de distância.", distance), "Ok");

                };
                map.Pins.Add(pino);
            }
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

        private void SwChangeView_Toggled(object sender, ToggledEventArgs e)
        {
            if (swChangeView.IsToggled)
            {
                map.MapType = MapType.Hybrid;
            }
            else
            {
                map.MapType = MapType.Street;
            }

        }


        private async void ImageButton_Tapped(object sender, EventArgs e)
        {
            DoAnimation(sender);
            //MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;            
            //p.Detail = new NavigationPage(new QRcodePage());          

            //Se a bicicleta não estiver bloqueada eu libero a realização de um novo scanner
            if (!bikeLocked)
            {
                ScannerAsync();
            }
            //caso contrário eu libero o desbloqueio da bicicleta.
            else
            {

                bool travado = await DataSeviceSmartBike.TravaBicicleta(usuarioLogado);
                bikeLocked = !travado;
                ImageButton.Source = @"qr_code.png";

                //Método para desbloquear a bicicleta
                //No retorno==tru mudar o ícone para o ícone padrão.
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

            var scannerPage = new ZXingScannerPage
            {
                Title = "Leitor de QR Code"
            };
            scannerPage.ToolbarItems.Add(item);
            await masterDt.Detail.Navigation.PopToRootAsync();
            scannerPage.OnScanResult += (result) =>
            {
                scannerPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    //Navigation.PopAsync();
                    if (result.Text.Contains("urn:ngsi-ld:bicicleta:"))
                    {
                        var duration = TimeSpan.FromMilliseconds(200);
                        Vibration.Vibrate(duration);
                        masterDt.Detail = new NavigationPage(new BicicletaInfoPage(masterDt, usuarioLogado, result.Text));
                    }
                    else
                    {
                        var duration = TimeSpan.FromMilliseconds(500);
                        Vibration.Vibrate(duration);
                        masterDt.Detail.Navigation.PopToRootAsync();
                        DisplayAlert("valor do QRcodeLidoInválido", result.Text, "OK");
                    }
                });
            };
            await Navigation.PushAsync(scannerPage);
        }


    }
}