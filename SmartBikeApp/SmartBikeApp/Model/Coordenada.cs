using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SmartBikeApp.Model
{
    public class Coordenada
    {
        private float latitude;
        private float longitude;

        public float Latitude { get => latitude; set => latitude = value; }
        public float Longitude { get => longitude; set => longitude = value; }

        public Coordenada(float Latitude, float Longitude)
        {
            this.Latitude = Latitude;
            this.Longitude = Longitude;
        }
        public Coordenada()
        {
        }

    }
}
