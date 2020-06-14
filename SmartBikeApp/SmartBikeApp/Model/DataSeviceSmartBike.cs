using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using ZXing;

namespace SmartBikeApp.Model
{
    public class DataSeviceSmartBike
    {
        struct BikeUserJSON
        {
            public string idUser;
            public string idBike;
            public bool status;
        }

        struct UserNoBikeJSON
        {
            public string idUser;
            public bool status;
        }

        struct ParkBikesJSON
        {
            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("dados")]
            public BicicletaCoordenadaJSON[] coordenadas { get; set; }
        }

        struct BicicletaCoordenadaJSON
        {
            [JsonProperty("coordinates")]
            public double[] Coordinates { get; set; }
        }

        struct UsuarioRetornoJSON
        {
            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("user")]
            public UserJSON User { get; set; }
        }

        struct UserJSON
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("firstName")]
            public string FirstName { get; set; }

            [JsonProperty("lastName")]
            public string LastName { get; set; }

            [JsonProperty("genero")]
            public string Genero { get; set; }

            [JsonProperty("nascimento")]
            public string Nascimento { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("imgBase64")]
            public string ImgBase64 { get; set; }

            [JsonProperty("totalRide")]
            public long TotalRide { get; set; }

            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("password")]
            public object Password { get; set; }

            [JsonProperty("token")]
            public string Token { get; set; }

            [JsonProperty("refPermission")]
            public string RefPermission { get; set; }
        }


        struct CorridaRetornoJSON
        {
            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("corridas")]
            public CorridaJSON[] Corridas { get; set; }
        }

        struct CorridaJSON
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("refUser")]
            public string RefUser { get; set; }

            [JsonProperty("refBike")]
            public string RefBike { get; set; }

            [JsonProperty("getBike")]
            public string GetBike { get; set; }

            [JsonProperty("leaveBike")]
            public string LeaveBike { get; set; }

            [JsonProperty("distancia")]
            public object Distancia { get; set; }

            [JsonProperty("duracao")]
            public double Duracao { get; set; }
        }

       struct BikeRetornoJson
        {
            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("bike")]
            public BikeJSON Bike { get; set; }
        }

        struct BikeJSON
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("modelo")]
            public string Modelo { get; set; }

            [JsonProperty("fabricante")]
            public string Fabricante { get; set; }

            [JsonProperty("velMax")]
            public long VelMax { get; set; }

            [JsonProperty("autonomia")]
            public string Autonomia { get; set; }

            [JsonProperty("peso")]
            public double Peso { get; set; }

            [JsonProperty("pesoMax")]
            public long PesoMax { get; set; }

            [JsonProperty("nivelAux")]
            public long NivelAux { get; set; }

            [JsonProperty("coordinates")]
            public BicicletaCoordenadaJSON Coordinates { get; set; }

            [JsonProperty("speed")]
            public long Speed { get; set; }

            [JsonProperty("batteryLevel")]
            public double BatteryLevel { get; set; }

            [JsonProperty("locked")]
            public bool Locked { get; set; }
        }

        public static async Task<User> GetUserFromAPIAsync(string usuario, string senha)
        {
            LoginUser login = new LoginUser();
            login.Username = usuario;
            login.Password = senha;
            UsuarioRetornoJSON usuarioRetorno = await GetDataFromServiceLogin(login).ConfigureAwait(false);

            if (usuarioRetorno.Status == "Sucess" && usuarioRetorno.User.Id != null)
            {
                User usuarioEncontrado = new User
                {
                    IdUser = usuarioRetorno.User.Id,
                    FirstName = usuarioRetorno.User.FirstName,
                    LastName = usuarioRetorno.User.LastName,
                    Genero = usuarioRetorno.User.Genero,
                    Nascimento = Convert.ToDateTime(usuarioRetorno.User.Nascimento),
                    Email = usuarioRetorno.User.Email,
                    ImgBase64 = usuarioRetorno.User.ImgBase64,
                    TotalRide = (float)usuarioRetorno.User.TotalRide,
                    Password = (string)usuarioRetorno.User.Password,
                    Token = usuarioRetorno.User.Token
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


        public static async Task<List<Coordenada>> RetornaBicicletasDisponiveis(User usuario)
        {            
            try
            {               
                List<Coordenada> lcoordenadas = new List<Coordenada>();
                ParkBikesJSON parkBikes = await GetBicicletasTravadas(usuario.Token).ConfigureAwait(false);
                if (parkBikes.Status == "Sucess")
                {

                    foreach (BicicletaCoordenadaJSON bicicletaCoordenada in parkBikes.coordenadas)
                    {
                        Coordenada coordenada = new Coordenada((float)bicicletaCoordenada.Coordinates[0], (float)bicicletaCoordenada.Coordinates[1]);
                        lcoordenadas.Add(coordenada);
                    }
                    return lcoordenadas;
                }
                throw new Exception("Erro no método");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }


        public static async Task<List<Model.Corrida>> GetHistoricoCorridas (User usuario)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            DateTimeStyles styles = DateTimeStyles.None;

            CorridaRetornoJSON corridaRetorno = await GetCorrida(usuario.IdUser, usuario.Token, 0).ConfigureAwait(false);
            List<Corrida> lcorrida = new List<Corrida>();
            if (corridaRetorno.Status == "Sucess" && corridaRetorno.Corridas.Length >= 1)
            {
                foreach (CorridaJSON corrida in corridaRetorno.Corridas)
                {                   
                    Corrida corridaClasse = new Corrida
                    {                        
                        IdCorrida = corrida.Id,
                        IdUser = corrida.RefUser,
                        IdBike = corrida.RefBike,
                        PegouBicicletaEm = DateTime.Parse(corrida.GetBike, culture, styles),  // Convert.ToDateTime(DateTime.ParseExact(corrida.GetBike.Replace('/','-'), "MM-dd-yyyy HH:mm:ss tt", CultureInfo.InvariantCulture)),
                        DevolveuBicicletaEm = DateTime.Parse(corrida.LeaveBike, culture, styles), // Convert.ToDateTime(DateTime.ParseExact(corrida.LeaveBike.Replace('/', '-'), "MM-dd-yyyy HH:mm:ss tt", CultureInfo.InvariantCulture)),
                        DistanciaPercorrida = 0, //não implementado ainda
                        Duracao = (float)corrida.Duracao
                    };
                    lcorrida.Add(corridaClasse);
                }
                return lcorrida;
            }
            else
            {
                return null;
            }
        }


        public static async Task<Bike> GetBikeInfo(string idBike, User usuario)
        {

            BikeRetornoJson bikeJSON = await GetInformationBike(idBike, usuario.Token).ConfigureAwait(false);

            if (bikeJSON.Status == "Sucess" && bikeJSON.Bike.Id != null)
            {
                Coordenada coordenada = new Coordenada((float)bikeJSON.Bike.Coordinates.Coordinates[0], (float)bikeJSON.Bike.Coordinates.Coordinates[1]);
                Bike bike = new Bike
                {
                    IdBike = bikeJSON.Bike.Id,
                    Modelo = bikeJSON.Bike.Modelo,
                    Fabricante = bikeJSON.Bike.Fabricante,
                    Coordenada = coordenada,
                    Speed = bikeJSON.Bike.VelMax,
                    Autonomia = bikeJSON.Bike.Autonomia,
                    BatteryLevel = (float)bikeJSON.Bike.BatteryLevel,
                    PesoBicicleta = (float)bikeJSON.Bike.Peso,
                    PesoMaximo = (float)bikeJSON.Bike.PesoMax,
                    NivelDeAuxilio = (int)bikeJSON.Bike.NivelAux,
                    Locked = bikeJSON.Bike.Locked
                };
                return bike;
            }
            else
            {
                return null;
            }
        }


        private static async Task<UsuarioRetornoJSON> GetDataFromServiceLogin(LoginUser loginUser)
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
                    //dynamic data = null;
                    UsuarioRetornoJSON usuarioRetornoJSON = JsonConvert.DeserializeObject<UsuarioRetornoJSON>(result);
                    return usuarioRetornoJSON;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no GetData from service Login");
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
                        BikeUserJSON bikeUser = new BikeUserJSON();
                        bikeUser.idBike = idBike;
                        bikeUser.idUser = idUser;
                        bikeUser.status = status;
                        json = JsonConvert.SerializeObject(bikeUser);
                    }
                    else
                    {
                        UserNoBikeJSON userNoBike = new UserNoBikeJSON();
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

        private static async Task<ParkBikesJSON> GetBicicletasTravadas(string token)
        {          
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    var url = String.Format("https://apitccsmartbike20200527201546.azurewebsites.net/api/bike/obterparque");

                    var response = await client.GetAsync(url);
                    string result = response.Content.ReadAsStringAsync().Result;
                    //dynamic data = null;
                    ParkBikesJSON park = JsonConvert.DeserializeObject<ParkBikesJSON>(result);

                    return park;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static async Task<CorridaRetornoJSON> GetCorrida(string idUser, string token, int quantidade)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var url = "";
                    if (quantidade == 0)
                    {
                        url = String.Format("https://apitccsmartbike20200527201546.azurewebsites.net/api/bike/corridahistorico/{0}", idUser);
                    }
                    else
                    {
                        url = String.Format("https://apitccsmartbike20200527201546.azurewebsites.net/api/bike/corridahistorico/{0}/{1}", idUser, quantidade);
                    }

                    var response = await client.GetAsync(url);
                    string result = response.Content.ReadAsStringAsync().Result;
                    //dynamic data = null;
                    CorridaRetornoJSON corrida = JsonConvert.DeserializeObject<CorridaRetornoJSON>(result);

                    return corrida;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static async Task<BikeRetornoJson> GetInformationBike(string idBike, string token)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var url = String.Format("https://apitccsmartbike20200527201546.azurewebsites.net/api/bike/obterbike/{0}", idBike);
                    var response = await client.GetAsync(url);
                    string result = response.Content.ReadAsStringAsync().Result;
                    BikeRetornoJson bike = JsonConvert.DeserializeObject<BikeRetornoJson>(result);
                    return bike;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
