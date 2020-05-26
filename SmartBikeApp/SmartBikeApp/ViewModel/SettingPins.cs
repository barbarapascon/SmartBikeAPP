using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace SmartBikeApp.ViewModel
{
    public class SettingPins
    {
        public List<Pin> pins = new List<Pin>();

        public SettingPins()
        {
            Pin bicicletarioEntrada = new Pin
            {
                Position = new Position(-23.735697, -46.583292 ),
                Label = "Entrada estacionamento",
                Address = "Bicicleatas disponíves: 5",
                Type = PinType.Place
            };
            
            Pin bicicletarioSaidaEstacionamento = new Pin
            {
                Position = new Position(-23.736220, -46.583468),
                Label = "Saída do estacionamento.",
                Address = "Bicicleatas disponíves: 15",
                Type = PinType.Place
            };

            Pin bicicletarioBlocoFaculdade = new Pin
            {
                Position = new Position(-23.739404, -46.583255),
                Label = "Proximo ao bloco da FTT",
                Address = "Bicicleatas disponíves: 1",
                Type = PinType.Place
            };

            Pin bicicletarioChafariz = new Pin
            {
                Position = new Position(-23.739232, -46.584224),
                Label = "Ao lado do Chafariz",
                Address = "Bicicleatas disponíves: 20",
                Type = PinType.Place
            };
            Pin bicicletarioOpostoRestaurante = new Pin
            {
                Position = new Position(-23.739125, -46.585073),
                Label = "Oposto ao Restaurante",
                Address = "Bicicleatas disponíves: 15",
                Type = PinType.Place
            };

            pins.Add(bicicletarioEntrada);
            pins.Add(bicicletarioSaidaEstacionamento);
            pins.Add(bicicletarioBlocoFaculdade);
            pins.Add(bicicletarioChafariz);
            pins.Add(bicicletarioOpostoRestaurante);
        }


    }
}

