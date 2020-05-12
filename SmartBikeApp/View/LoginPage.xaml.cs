﻿using SmartBikeApp.View;
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

        private void lbEsqueciSenha_Tapped(object sender, EventArgs e)
        {

        }

        private void Logar_Clicked(object sender, EventArgs e)
        {
            NavigationPage.SetHasNavigationBar(this, true);
            Application.Current.MainPage = new Main();
            //Navigation.PushAsync(new PagePrincipal());
            Navigation.PushModalAsync(new Main());

            /*
             * MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
            p.Detail = new NavigationPage(new Sobre());
            p.IsPresented = false;
            */
        }
    }
}