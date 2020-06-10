using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace SmartBikeApp.Model
{
    public class DataSeviceSmartBike
    {
        struct BikeUser
        {
            public string idUser;
            public string idBike;
            public bool status;
        }

        struct UserNoBike
        {
            public string idUser;
            public bool status;
        }

        public static async Task<User> GetUserFromAPIAsync(string usuario, string senha)
        {
            LoginUser login = new LoginUser();
            login.Username = usuario;
            login.Password = senha;
            dynamic resultado = await GetDataFromServiceLogin(login).ConfigureAwait(false);
            if (resultado["id"] != null)
            {
                User usuarioEncontrado = new User
                {
                    IdUser = (string)resultado["id"],
                    FirstName = (string)resultado["firstName"],
                    LastName = (string)resultado["lastName"],
                    Genero = (string)resultado["genero"],
                    Nascimento = (DateTime)resultado["nascimento"],
                    Email = (string)resultado["email"],
                    ImgBase64 = (string)resultado["imgBase64"],
                    TotalRide = (float)resultado["totalRide"],
                    Username = (string)resultado["username"],
                    Password = (string)resultado["password"],
                    Token = (string)resultado["token"]
                };
                return usuarioEncontrado;
            }
            else
            {
                return null;
            }
        }


        public static async Task<bool> TravaBicicleta(User usuario)
        {
            dynamic resultado = await GenericLocker(usuario.IdUser, null, usuario.Token, true).ConfigureAwait(false);
            if ((string)resultado["status"] == "Sucess")
            {
                return true;
            }
            else
            {
                throw new Exception((string)resultado["message"]);
            }
        }


        public static async Task<bool> DestravaBicicleta(User usuario, string IDBike)
        {
            dynamic resultado = await GenericLocker(usuario.IdUser, IDBike, usuario.Token, false).ConfigureAwait(false);
            if ((string)resultado["status"] == "Sucess")
            {
                return true;
            }
            else
            {
                throw new Exception((string)resultado["message"]);
            }
        }

        public static async Task<bool> VerificaUsuarioCorrida(User usuario)
        {
            try
            {
                dynamic resultado = await UsuarioEmCorrida(usuario.IdUser, usuario.Token).ConfigureAwait(false);
                if((string)resultado["status"] == "Sucess")
                {
                    string retorno = (((string)resultado["message"]).ToLower());
                    return Convert.ToBoolean(retorno);
                    
                }
                throw new Exception("Erro no método");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        private static async Task<dynamic> GetDataFromServiceLogin(LoginUser loginUser)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    var url = "https://apitccsmartbike20200527201546.azurewebsites.net/api/account/authenticate";

                    string json = JsonConvert.SerializeObject(loginUser);
                    var dataContent = new StringContent(json, Encoding.UTF8, "application/json");

                    //client.BaseAddress = new Uri("https://apitccsmartbike20200527201546.azurewebsites.net/api/account/authenticate");
                    //client.DefaultRequestHeaders.Accept.Clear();

                    /*client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "xxxxxxxxxxxxxxxxxxxx");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("X-Version", "1");
                    */
                    //client.DefaultRequestHeaders.Add("Content-Type", "application/json");


                    var response = await client.PostAsync(url, dataContent);
                    string result = response.Content.ReadAsStringAsync().Result;
                    dynamic data = null;
                    data = JsonConvert.DeserializeObject(result);
                    return data;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static async Task<dynamic> GenericLocker(string idUser, string idBike, string token, bool status)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    string json;
                    if (idBike != null)
                    {
                        BikeUser bikeUser = new BikeUser();
                        bikeUser.idBike = idBike;
                        bikeUser.idUser = idUser;
                        bikeUser.status = status;
                        json = JsonConvert.SerializeObject(bikeUser);
                    }
                    else
                    {
                        UserNoBike userNoBike = new UserNoBike();
                        userNoBike.idUser = idUser;
                        userNoBike.status = status;
                        json = JsonConvert.SerializeObject(userNoBike);
                    }
                    
                    var url = "https://apitccsmartbike20200527201546.azurewebsites.net/api/bike/alterarstatus";                    
                    var dataContent = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, dataContent);
                    string result = response.Content.ReadAsStringAsync().Result;
                    dynamic data = null;
                    data = JsonConvert.DeserializeObject(result);
                    return data;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private static async Task<dynamic> UsuarioEmCorrida(string idUsuario, string token)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    var url = String.Format("https://apitccsmartbike20200527201546.azurewebsites.net/api/bike/checkuserinrun/{0}", idUsuario);
                    
                    var response = await client.GetAsync(url);
                    string result = response.Content.ReadAsStringAsync().Result;
                    dynamic data = null;
                    data = JsonConvert.DeserializeObject(result);

                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
