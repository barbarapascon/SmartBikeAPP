using SmartBikeApp.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartBikeApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoricoPage : ContentPage
    {
        User usuarioLogado;
        public HistoricoPage(User usuario)
        {
            usuarioLogado = usuario;
            InitializeComponent();
            GetHistorico();
        }


        public async void GetHistorico()
        {
            List<Corrida> listaDeCorrida = await DataSeviceSmartBike.GetHistoricoCorridas(usuarioLogado);
            if (listaDeCorrida != null)
            {
                foreach (Corrida corrida in listaDeCorrida)
                {

                    string distanciaPercorrida = String.Format("{0} km", corrida.DistanciaPercorrida.ToString());
                    double minutos;
                    string tempoPedalado;
                    if (corrida.Duracao >= 60)
                    {
                        double horas = Math.Round(corrida.Duracao / 60, 0);
                        minutos = Math.Round(corrida.Duracao % 60, 0);

                        tempoPedalado = String.Format("{0} h {1} min", horas, minutos);
                    }
                    else
                    {
                        minutos = Math.Round(corrida.Duracao % 60, 0);

                        tempoPedalado = String.Format("{0} min", minutos);
                    }
                    PreparaTelaHistorico(tempoPedalado, distanciaPercorrida, corrida.DevolveuBicicletaEm);
                }
            }
            else
            {
                PreparaTelaSemHistorico();
                //await DisplayAlert("Corrida não encontrada.", "Você não realizou nehuma corrida ainda, utilize uma de nossas e-bikes para chegar ao seu destinho mais rápido!", "OK");
            }
        }
        public void PreparaTelaHistorico(string tempoPedalado, string distanciPercorrida, DateTime date)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            int dia = date.Day;
            int ano = date.Year;
            string mes = culture.TextInfo.ToTitleCase(dtfi.GetMonthName(date.Month));

            string dataExtenso = dia + " de " + mes + " de " + ano + ", São Barnardo do Campo";

            Frame cardHistoryFrame = new Frame
            {
                CornerRadius = 15,
                HeightRequest = 130,
                WidthRequest = 1500,
                Padding = 0,
                Margin = new Thickness(10, 10, 10, 0),
                HorizontalOptions = LayoutOptions.Start,
                IsClippedToBounds = true,
                HasShadow = true,
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Padding = new Thickness(10, 15, 10, 15),
                    Children =
                    {
                        new StackLayout
                        {
                            Spacing = 0,
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                 new Image {
                                    Source = @"bicicleta2.png",
                                    HorizontalOptions= LayoutOptions.Start,
                                    VerticalOptions= LayoutOptions.Start,
                                    Aspect=Aspect.AspectFill,
                                    HeightRequest=45,
                                    WidthRequest=45,
                                    Margin=new Thickness(10,0,5,0)
                                },
                                 new StackLayout
                                 {
                                     Orientation= StackOrientation.Vertical,
                                     HorizontalOptions=LayoutOptions.StartAndExpand,
                                     Margin=new Thickness(30,0,15,0),
                                     Children =
                                     {
                                         new StackLayout
                                         {
                                             Orientation= StackOrientation.Horizontal,
                                             HorizontalOptions= LayoutOptions.StartAndExpand,
                                             VerticalOptions= LayoutOptions.Start,
                                             Children =
                                             {
                                                 new Image{
                                                     Source=@"desbloqueado.png",
                                                     Aspect=Aspect.AspectFit,
                                                     HeightRequest=20,
                                                     WidthRequest=20,
                                                     Margin=new Thickness(5,0),
                                                 },
                                                 new Label {
                                                     Text="Localização desconhecida",
                                                     FontSize=12.5,
                                                     FontAttributes= FontAttributes.Italic,
                                                     TextColor=Color.Gray,
                                                     VerticalOptions=LayoutOptions.Center
                                                 }

                                             }
                                         },
                                         new StackLayout
                                         {
                                             Orientation= StackOrientation.Horizontal,
                                             HorizontalOptions= LayoutOptions.StartAndExpand,
                                             VerticalOptions= LayoutOptions.Start,
                                             Children =
                                             {
                                                  new Image{
                                                     Source=@"trava.png",
                                                     HorizontalOptions=LayoutOptions.Start,
                                                     VerticalOptions= LayoutOptions.Start,
                                                     Aspect= Aspect.AspectFill,
                                                     HeightRequest=20,
                                                     WidthRequest=20,
                                                     Margin=new Thickness(5,0),
                                                 },
                                                 new Label {
                                                     Text="Localização desconhecida",
                                                     HorizontalOptions= LayoutOptions.CenterAndExpand,
                                                     VerticalOptions=LayoutOptions.CenterAndExpand,
                                                     FontSize=12.5,
                                                     FontAttributes= FontAttributes.Italic,
                                                     TextColor=Color.Gray,
                                                 }
                                             }
                                         },
                                        new Label
                                        {
                                            Text= dataExtenso,
                                            HorizontalOptions= LayoutOptions.Start,
                                            VerticalOptions= LayoutOptions.Center,
                                            FontSize=9,
                                            FontAttributes=  FontAttributes.Italic,
                                            TextColor= Color.FromHex("#A258C7"),
                                            Margin=new Thickness(4,0,4,0)
                                        }
                                    }

                                 }
                            }
                        },
                        new StackLayout
                        {
                            Orientation=StackOrientation.Horizontal,
                            HorizontalOptions=LayoutOptions.EndAndExpand,
                            Margin=new Thickness(0,10, 15, 5),
                            Children = {
                                new Image {
                                    Source = @"relogio.png",
                                    HeightRequest=20,
                                    WidthRequest=20,
                                },
                                new Label
                                {
                                   Text= tempoPedalado,
                                   FontSize=15,
                                   TextColor=Color.Gray,
                                   Margin=new Thickness(5, 0, 10, 0),
                                },
                                /* new Image {
                                    Source = @"estrada.png",
                                    HeightRequest=20,
                                    WidthRequest=20,
                                },

                                new Label
                                {
                                    Text= distanciPercorrida,
                                    FontSize= 15,
                                    TextColor=Color.Gray,
                                    Margin=new Thickness(5, 0, 5, 0)
                                },*/
                             }
                        }

                    }

                }
            };
            stackHistorico.Children.Add(cardHistoryFrame);
        }
        public void PreparaTelaSemHistorico()
        {

            Frame cardNoHistoryFrame = new Frame
            {
                CornerRadius = 15,
                Padding = 0,
                Margin = new Thickness(10, 10, 10, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsClippedToBounds = true,
                HasShadow = true,
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Padding = new Thickness(10, 15, 10, 15),
                    Children =
                    {
                        new Label {
                            Text = "Você não realizou nenhuma corrida!",
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center,
                            FontSize = 20,
                            TextColor = Color.FromHex("#A258C7"),
                            Margin = new Thickness(5, 0, 5, 0)
                        },
                        new Label {
                            Text = "Utilize uma de nossas e-bikes para chegar ao seu destinho mais rápido!",
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center,
                            FontSize = 15,
                            TextColor = Color.FromHex("#A258C7"),
                            Margin = new Thickness(5, 0, 5, 0)
                        },

                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                new Image {
                                        Source = @"parkedbicicleta.png",
                                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                                        VerticalOptions = LayoutOptions.CenterAndExpand,
                                        Aspect = Aspect.AspectFit,
                                        HeightRequest = 300,
                                        WidthRequest = 300,
                                        Margin = new Thickness(10,0,5,0)}
                            }
                        }
                    }
                }
            };
            stackHistorico.Children.Add(cardNoHistoryFrame);
        }
    }
}