using SmartBikeApp.Interfaces;
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
    public partial class BicicletaInfoPage : ContentPage
    {
        readonly MasterDetailPage masterDt;
        public BicicletaInfoPage(MasterDetailPage masterDetail)
        {
            masterDt = masterDetail;
            InitializeComponent();
        }

        private void LockBike_Tapped(object sender, EventArgs e)
        {
            DoAnimation(sender);
            DependencyService.Get<INotification>().CreateNotification("Obrigado por escolher uma e-bike.", "Pedale com segurança utilizando capacete.");
            
            masterDt.Detail = new NavigationPage(new FindBikesPage(masterDt, true));
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

    }
}