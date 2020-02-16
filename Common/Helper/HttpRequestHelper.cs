using System;
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

namespace Common.Helpers
{
    public class HttpRequestHelper
    {
        public HttpClient client = new HttpClient();
        public static string QueryString<T>(string url, T data){
            var properties = from property in data.GetType().GetProperties()
                             where property.GetValue(data, null) != null
                             select property.Name + "=" + HttpUtility.UrlEncode(property.GetValue(data, null).ToString());
            url += "?"+String.Join("&", properties.ToArray());
            return url;
        }
        public dynamic HttpRequestGet<T>(string url, T data) {
            return HttpRequestGet(QueryString(url, data));
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
            HttpResponseMessage response = client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")).Result;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return  JsonConvert.DeserializeObject<dynamic>(
                response.Content.ReadAsStringAsync().Result
            );
        }
    }
}
