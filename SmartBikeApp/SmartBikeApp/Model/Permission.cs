using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBikeApp.Model
{
    public class Permission
    {
        private string idPermission;
        private string descricao;
        public string IdPermission { get => idPermission; set => idPermission = value; }
        public string Descricao { get => descricao; set => descricao = value; }

        public Permission(string IDPermission, string Descricao)
        {
            this.IdPermission = IDPermission;
            this.Descricao = Descricao;
        }
        public Permission() { }
    }
}
