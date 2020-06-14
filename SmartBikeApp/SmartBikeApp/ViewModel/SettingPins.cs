using SmartBikeApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace SmartBikeApp.ViewModel
{
    public class SettingPins
    {
        public List<Pin> pins = new List<Pin>();


        Pin bicicletarioEntrada = new Pin
        {
            Position = new Position(-23.735697, -46.583292),
            Label = "Entrada estacionamento",
            Address = "0",
            Type = PinType.Place
        };

        Pin bicicletarioSaidaEstacionamento = new Pin
        {
            Position = new Position(-23.736220, -46.583468),
            Label = "Saída do estacionamento.",
            Address = "0",
            Type = PinType.Place
        };

        Pin bicicletarioBlocoFaculdade = new Pin
        {
            Position = new Position(-23.739404, -46.583255),
            Label = "Proximo ao bloco da FTT",
            Address = "0",
            Type = PinType.Place
        };

        Pin bicicletarioChafariz = new Pin
        {
            Position = new Position(-23.739232, -46.584224),
            Label = "Ao lado do Chafariz",
            Address = "0",
            Type = PinType.Place
        };
        Pin bicicletarioOpostoRestaurante = new Pin
        {
            Position = new Position(-23.739125, -46.585073),
            Label = "Oposto ao Restaurante",
            Address = "0",
            Type = PinType.Place
        };


        public void SetPinsConhecidos()
        {
            pins.Add(bicicletarioEntrada);
            pins.Add(bicicletarioSaidaEstacionamento);
            pins.Add(bicicletarioBlocoFaculdade);
            pins.Add(bicicletarioChafariz);
            pins.Add(bicicletarioOpostoRestaurante);

        }
        private void AtualizaQuantidadeBicicletas(Pin pin, int quantidadeNova)
        {
            pin.Address = pin.Address + quantidadeNova.ToString();
        }
        private void CriaLocalizacaoDesconhecida(Position position)
        {
            Pin Localizacaodesconhecida = new Pin
            {
                Position = position,
                Label = String.Format("Localização desconhecida"),
                Address = "1",
                Type = PinType.Place
            };
            pins.Add(Localizacaodesconhecida);
        }


        public SettingPins(List<Coordenada> coordenadas)
        {
            SetPinsConhecidos();
            bool estaProximoAumPinExistente;


            foreach (Coordenada coordenada in coordenadas)
            {
                estaProximoAumPinExistente = false;

                foreach (Pin pinAtual in pins)
                {
                    Position posicaoNova = new Position(coordenada.Latitude, coordenada.Longitude);
                    double distaciaAtePinConhecido = Math.Round(Distance.BetweenPositions(pinAtual.Position, posicaoNova).Meters, 0);
                    if (distaciaAtePinConhecido <= 50)
                    {
                        int quantidade = Convert.ToInt32(pinAtual.Address) + 1;
                        pinAtual.Address = String.Format("{0}", quantidade);
                        estaProximoAumPinExistente = true;
                    }
                    if ((pins.IndexOf(pinAtual) == pins.Count - 1) && !estaProximoAumPinExistente)
                    {
                        CriaLocalizacaoDesconhecida(posicaoNova);
                        break;
                    }
                }
            }
            //Reescreve o endereço de uma forma mais interessante para o usuário, definindo quantas bicicletas estão disponíveis.
            foreach (Pin pinAtual in pins)
            {
                pinAtual.Address = String.Format("Bicicletas disponíveis: {0}", pinAtual.Address);
            }
        }
    }
}

