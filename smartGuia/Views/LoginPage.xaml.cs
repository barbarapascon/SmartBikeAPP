using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace smartGuia.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private void Btn_clickedLog(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainPage();

        }
    }
}
