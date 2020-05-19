using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBikeApp.Model
{
    public class Tempo
    {
        public string Title { get; set; }
        public string Temperature { get; set; }
        public string Wind { get; set; }
        public string Humidity { get; set; }
        public string Visibility { get; set; }
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
        public Tempo()
        {
            // Como as Labels se vinculam a estes valores vamos defini-los 
            // como uma string vazia no construtor 
            this.Title = " ";
            this.Temperature = " ";
            this.Wind = " ";
            this.Humidity = " ";
            this.Visibility = " ";
            this.Sunrise = " ";
            this.Sunset = " ";
        }
    }
}
