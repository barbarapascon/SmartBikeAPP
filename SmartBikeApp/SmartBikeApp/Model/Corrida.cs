using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBikeApp.Model
{
    public class Corrida
    {
        private string idCorrida;
        private string idUser;
        private string idBike;
        private DateTime pegouBicicletaEm;
        private DateTime devolveuBicicletaEm;
        private float distanciaPercorrida;
        private float duracao;

        public string IdCorrida { get => idCorrida; set => idCorrida = value; }
        public string IdUser { get => idUser; set => idUser = value; }
        public string IdBike { get => idBike; set => idBike = value; }
        public DateTime PegouBicicletaEm { get => pegouBicicletaEm; set => pegouBicicletaEm = value; }
        public DateTime DevolveuBicicletaEm { get => devolveuBicicletaEm; set => devolveuBicicletaEm = value; }
        public float DistanciaPercorrida { get => distanciaPercorrida; set => distanciaPercorrida = value; }
        public float Duracao { get => duracao; set => duracao = value; }

        public Corrida(string IDCorrida, string IDUser, string IDBike, DateTime PegouBicicletaEm, DateTime DevolveuBicicletaEm, float DistanciaPercorrida, float Duracao)
        {
            this.IdCorrida = IDCorrida;
            this.IdUser = IDUser;
            this.IdBike = IDBike;
            this.PegouBicicletaEm = PegouBicicletaEm;
            this.DevolveuBicicletaEm = DevolveuBicicletaEm;
            this.DistanciaPercorrida = DistanciaPercorrida;
            this.Duracao = Duracao;
        }
        public Corrida() { }

    }
}
