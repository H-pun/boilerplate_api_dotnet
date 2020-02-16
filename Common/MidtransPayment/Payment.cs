using System;
using System.IO;
using System.Web;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;

namespace Common.MidtransPayment
{
    public class Payment
    {
        public HttpClient client = new HttpClient();
        public dynamic HttpRequestGet<T>(string url, T data) {
            var properties = from result in data.GetType().GetProperties()
                             where result.GetValue(data, null) != null
                             select result.Name + "=" + HttpUtility.UrlEncode(result.GetValue(data, null).ToString());
            url += "?"+String.Join("&", properties.ToArray());
            return HttpRequestGet(url);
        }

        public dynamic HttpRequestGet(string url) {
            HttpResponseMessage response = client.GetAsync(url).Result;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return JsonConvert.DeserializeObject<dynamic>(
                response.Content.ReadAsStringAsync().Result
            ); 
        }

        public dynamic HttpRequestPost<T>(string url, T data) {
            HttpResponseMessage response = client.PostAsync(
                url,
                new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json") 
            ).Result;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return  JsonConvert.DeserializeObject<dynamic>(
                response.Content.ReadAsStringAsync().Result
            );
        }

        public Payment() {
            var username = ConfigurationManager.AppSettings["UsernameMidtrans"];
            var password = ConfigurationManager.AppSettings["PasswordMidtrans"];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["UriMidtrans"]);
        }
    }
}
