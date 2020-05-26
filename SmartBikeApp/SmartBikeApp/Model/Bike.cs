using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmartBikeApp.Model
{
    public class Bike
    {
        private string idBike;
        private string modelo;
        private string fabricante;
        private Coordenada coordenada;
        private float speed;
        private float batteryLevel;
        private bool locked;

        public string IdBike { get => idBike; set => idBike = value; }
        public string Modelo { get => modelo; set => modelo = value; }
        public string Fabricante { get => fabricante; set => fabricante = value; }
        public Coordenada Coordenada { get => coordenada; set => coordenada = value; }
        public float Speed { get => speed; set => speed = value; }
        public float BatteryLevel { get => batteryLevel; set => batteryLevel = value; }
        public bool Locked { get => locked; set => locked = value; }

        public Bike(string IDBike, string Modelo, string Fabricante, float latitude, float longitude, float Speed, float BatteryLevel, bool Locked)
        {
            this.IdBike = IDBike;
            this.Modelo = Modelo;
            this.Fabricante = Fabricante;
            this.Coordenada = new Coordenada(latitude, longitude);
            this.Speed = Speed;
            this.BatteryLevel = BatteryLevel;
            this.Locked = Locked;
        }
        public Bike() { }

    }
}
