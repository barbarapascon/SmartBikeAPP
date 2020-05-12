using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace SmartBikeApp
{
   public class CustomPin : Pin
    {
        public string ID { get; set; }
        public string Url { get; set; }
    }
}
