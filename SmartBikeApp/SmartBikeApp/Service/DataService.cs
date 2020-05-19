using SmartBikeApp.Model;
using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartBikeApp.Service
{
    public class DataService
    {
        public static async Task<Tempo> GetPrevisaoDoTempo(string cidade)
        {
            string appId = "1bcc27831f842c43011d83d43a8db4c0";

            string queryString = "http://api.openweathermap.org/data/2.5/weather?q=" + cidade + "&units=metric" + "&appid=" + appId;
            dynamic resultado = await DataService.getDataFromService(queryString).ConfigureAwait(false);
            if (resultado["weather"] != null)
            {
                Tempo previsao = new Tempo();
                previsao.Title = (string)resultado["name"];
                previsao.Temperature = (string)(resultado["main"]["temp"])+ " C";
                previsao.Wind = (string)resultado["wind"]["speed"] + " mph";
                previsao.Humidity = (string)resultado["main"]["humidity"] + " %";
                previsao.Visibility = (string)resultado["weather"][0]["main"];
                DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                DateTime sunrise = time.AddSeconds((double)resultado["sys"]["sunrise"]);
                DateTime sunset = time.AddSeconds((double)resultado["sys"]["sunset"]);
                previsao.Sunrise = String.Format("{0:d/MM/yyyy HH:mm:ss}", sunrise);
                previsao.Sunset = String.Format("{0:d/MM/yyyy HH:mm:ss}", sunset);
                return previsao;
            }
            else
            {
                return null;
            }
        }

        public static async Task<Tempo> GetPrevisaoDoTempoGeoLocation(string lat, string longi)
        {
            string appId = "1bcc27831f842c43011d83d43a8db4c0";            
            string queryString = "http://api.openweathermap.org/data/2.5/weather?lat="+lat+"&lon="+longi+"&appid=" + appId; 
            dynamic resultado = await DataService.getDataFromService(queryString).ConfigureAwait(true);
            if (resultado["weather"] != null)
            {
                double temperaturaCelcius = 0;
                double velocidadeVento = 0;
                Tempo previsao = new Tempo();
                previsao.Title = (string)resultado["name"];

                temperaturaCelcius = Math.Round((Convert.ToDouble(resultado["main"]["temp"]) - 273.15), 0);
                velocidadeVento = Math.Round((Convert.ToDouble(resultado["wind"]["speed"]) * 1.609344),0);
                previsao.Temperature = temperaturaCelcius.ToString() + " ºC"; 

                previsao.Wind = velocidadeVento.ToString() + " km/h";
                previsao.Humidity = (string)resultado["main"]["humidity"] + " %";
                previsao.Visibility = (string)resultado["weather"][0]["main"];
                DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                DateTime sunrise = time.AddSeconds((double)resultado["sys"]["sunrise"]);
                DateTime sunset = time.AddSeconds((double)resultado["sys"]["sunset"]);
                previsao.Sunrise = String.Format("{0:d/MM/yyyy HH:mm:ss}", sunrise);
                previsao.Sunset = String.Format("{0:d/MM/yyyy HH:mm:ss}", sunset);
                return previsao;
            }
            else
            {
                return null;
            }
        }

        public static async Task<dynamic> getDataFromService(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);
            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject(json);
            }
            return data;
        }
        public static async Task<dynamic> getDataFromServiceByCity(string city)
        {
            string appId = "1bcc27831f842c43011d83d43a8db4c0";
            string url = string.Format("http://api.openweathermap.org/data/2.5/forecast/daily?q={0}&units=metric&cnt=1&APPID={1}", city.Trim(), appId);
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject(json);
            }
            return data;
        }
    }
}
