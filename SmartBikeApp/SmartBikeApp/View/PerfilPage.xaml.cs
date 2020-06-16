using SmartBikeApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartBikeApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilPage : ContentPage
    {
        
        public PerfilPage(User userLogged)
        {
            InitializeComponent();
            FirstName.Text = userLogged.FirstName;
            LastName.Text = userLogged.LastName;
            idade.Text = (DateTime.Now.Year - userLogged.Nascimento.Year).ToString();
            if(userLogged.Genero == "M")
            {
                Genero.Text = "Masculino";
            }
            else if(userLogged.Genero == "F")
            {
                Genero.Text = "Feminino";
            }
            else
            {
                Genero.Text = "Não é binário";
            }
            email.Text = userLogged.Email;
            //totalPedalado.Text = String.Format("{0} km",userLogged.TotalRide.ToString());
            byte[] Base64Stream = Convert.FromBase64String(userLogged.ImgBase64);
            User_image.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
            
        }
    }
}