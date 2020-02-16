using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Notification
    {
        static string FireBaseKey = ConfigurationManager.AppSettings["FirebaseKey"];

        static string FireBaseSender = "301961012673";

        static string UrlFirebase = "https://fcm.googleapis.com/fcm/send";

        public class MessageAndroid
        {
            public string[] registration_ids { get; set; }
            public NotifAndroid data { get; set; }
        }

        public class NotifAndroid
        {
            public string title { get; set; }
            public string text { get; set; }
            public string priority { get { return "high"; } }
            public object additional_data { get; set; }
        }

        public class MessageIos
        {
            public string[] registration_ids { get; set; }
            public NotifIos notification { get; set; }
            public object data { get; set; }
            public string priority { get { return "high"; } }
        }

        public class NotifIos
        {
            public string title { get; set; }
            public string body { get; set; }
            public string category { get { return "CustomBB8Push"; } }
            public bool mutable_content { get { return true; } }
        }

        public class TargetNotif
        {
            public string deviceToken { get; set; }
            public string deviceType { get; set; }
        }

        public static void SendNotif(List<TargetNotif> targetNotifs, string title, string body, object data = null)
        {
            try
            {
                var authorizationKey = string.Format("key={0}", FireBaseKey);
                var senderId = string.Format("id={0}", FireBaseSender);

                var messageInformationAndroid = new MessageAndroid()
                {
                    registration_ids = targetNotifs.Where(t => t.deviceType == "androidOS").Select(t => t.deviceToken).ToArray(),
                    data = new NotifAndroid()
                    {
                        title = title,
                        text = body,
                        additional_data = data
                    }
                };

                var messageInformationIos = new MessageIos()
                {
                    registration_ids = targetNotifs.Where(t => t.deviceType == "iOS").Select(t => t.deviceToken).ToArray(),
                    notification = new NotifIos()
                    {
                        title = title,
                        body = body
                    },
                    data = data
                };

                var jsonBodyAndroid = JsonConvert.SerializeObject(messageInformationAndroid);
                var sendNotifandroid = pushNotifFirebaseAsync(jsonBodyAndroid, authorizationKey, senderId);

                var jsonBodyIos = JsonConvert.SerializeObject(messageInformationIos);
                var sendNotifIos = pushNotifFirebaseAsync(jsonBodyIos, authorizationKey, senderId);
            }
            catch { throw; }
        }

        private static async Task pushNotifFirebaseAsyncBackup(string jsonBody, string authorizationKey)
        {
            HttpRequestMessage httpRequest = null;
            HttpClient httpClient = null;

            try
            {
                httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send");

                httpRequest.Headers.TryAddWithoutValidation("Authorization", authorizationKey);
                httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage result;
                using (var client = new HttpClient())
                {
                    result = await client.SendAsync(httpRequest);
                }

                //return result.IsSuccessStatusCode;
            }
            catch { throw; }
            finally
            {
                httpRequest.Dispose();
                httpClient.Dispose();
            }
        }

        private static async Task pushNotifFirebaseAsync(string jsonBody, string authorizationKey, string senderId)
        {
            try
            {
                WebRequest tRequest = WebRequest.Create(UrlFirebase);
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                Byte[] byteArray = Encoding.UTF8.GetBytes(jsonBody);
                tRequest.Headers.Add(string.Format("Authorization: {0}", authorizationKey));
                tRequest.Headers.Add(string.Format("Sender: {0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = await tReader.ReadToEndAsync();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch { throw; }
        }
    }
}
