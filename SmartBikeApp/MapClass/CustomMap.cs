using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace SmartBikeApp
{
    public class CustomMap : Map
    {
        public List<CustomPin> customPins { get; set; }

        public static implicit operator ContentPage(CustomMap v)
        {
            throw new NotImplementedException();
        }
    }
}
