using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBikeApp.Model
{
    public class User
    {
        private string idUser;
        private string firstName;
        private string lastName;
        private string genero;
        private DateTime nascimento;
        private string email;
        private string imgBase64;
        private float totalRide;
        private string username;
        private string password;
        private string token;

        public string IdUser { get => idUser; set => idUser = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Genero { get => genero; set => genero = value; }
        public DateTime Nascimento { get => nascimento; set => nascimento = value; }
        public string Email { get => email; set => email = value; }
        public string ImgBase64 { get => imgBase64; set => imgBase64 = value; }
        public float TotalRide { get => totalRide; set => totalRide = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Token { get => token; set => token = value; }


        public User (string IDUser, string FirstName, string Genero, DateTime Nascimento, string Email, string ImgBase64, float TotalRide, string Username, string Password, string Token)
        {
            this.IdUser = IDUser;
            this.FirstName = FirstName;
            this.Genero = Genero;
            this.Nascimento = Nascimento;
            this.Email = Email;
            this.ImgBase64 = ImgBase64;
            this.TotalRide = TotalRide;
            this.Username = Username;
            this.Password = Password;
            this.token = Token;
        }
        public User() { }
    }
}
