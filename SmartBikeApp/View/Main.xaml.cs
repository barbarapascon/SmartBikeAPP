using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Geolocator;
using SmartBikeApp.Model;
using SmartBikeApp.Service;
//using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Markup;
using Xamarin.Forms.Xaml;

namespace SmartBikeApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : MasterDetailPage
    {
        public Main()
        {
            GetLocation();
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            this.BindingContext = new Tempo();
            
        }

        private async void GetLocation()
        {
            try
            {
                //-23.7544463,-46.5548277 --Minha Casa
                //-23.7359221,-46.5854127 --Faculdade
                //-23.738945, -46.584272 --Teatro Salvador arena
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(100));
                if (CrossGeolocator.Current.IsGeolocationEnabled)
                {   
                    Tempo previsaoDoTempo = await DataService.GetPrevisaoDoTempoGeoLocation(position.Latitude.ToString(), position.Longitude.ToString());
                    //PreparaMapa(position.Latitude, position.Latitude, previsaoDoTempo.Title);                    
                    this.BindingContext = previsaoDoTempo;
                    Position position1 = new Position(position.Latitude, position.Longitude);
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(position1, Distance.FromMeters(15));
                    mapp.MoveToRegion(mapSpan);
                }
                else
                {
                    await DisplayAlert("Message", "GPS not enabled", "OK");
                }
             
            }
            catch(Exception err)
            {
                await DisplayAlert("Erro no serviço de previsão do tempo", err.Message, "OK");
            }
        }
        private void PreparaMapa(double lat, double longe, string cidade)
        {
            /*Map part*/
            var customMap = new CustomMap
            {
                MapType = MapType.Street/*,
                WidthRequest = App.ScreenWidth,
                HeightRequest = App.ScreenHeight*/

            };
            var pin = new CustomPin
            {
                Type = PinType.Place,
                Position = new Position(lat, longe),
                Label = "Localização atual",
                Address = cidade,
                ID = "Localização atual",
                Url = "http://xamarin.com/about"
            };

            var pin2 = new CustomPin
            {
                Type = PinType.Place,
                Position = new Position(-23.7398312, -46.5551412),
                Label = "Localização atual",
                Address = cidade,
                ID = "Localização atual",
                Url = "http://xamarin.com/about"
            };

            customMap.customPins = new List<CustomPin> { pin };
            customMap.Pins.Add(pin);
            customMap.Pins.Add(pin2);
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, longe), Distance.FromKilometers(1)));
            mapContent = customMap;
        }
    }
}