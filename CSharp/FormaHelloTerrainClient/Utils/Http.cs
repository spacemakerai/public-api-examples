using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FormaHelloTerrainClient
{
    public class Http
    {

        public static T Get<T>(string url, string token)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Headers.Add("x-ads-region", "EMEA");
            request.Accept = "application/json";
            request.Method = "GET";

            Stream stream = request.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(stream);

            string line = sr.ReadToEnd();

            stream.Close();
            sr.Close();

            T t= JsonConvert.DeserializeObject<T>(line);

            return t;
        }

        public static T Post<T>(string url, object body, string token)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Headers.Add("x-ads-region", "EMEA");
            request.Accept = "application/json";
            request.Method = "POST";
            request.ContentType = "application/json";

            string bodyString = JsonConvert.SerializeObject(body, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] byte1 = encoding.GetBytes(bodyString);
            request.ContentLength = byte1.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byte1, 0, byte1.Length);
            }

            try
            {
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);

                string line = sr.ReadToEnd();

                stream.Close();
                sr.Close();

                return JsonConvert.DeserializeObject<T>(line);
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    // TODO: Handle response being null
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        Console.WriteLine(text);
                    }
                }
                throw e;
            }
        }

        public static T Put<T>(string url, object body, string token)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Headers.Add("x-ads-region", "EMEA");
            request.Accept = "application/json";
            request.Method = "PUT";
            request.ContentType = "application/json";

            string bodyString = JsonConvert.SerializeObject(body, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] byte1 = encoding.GetBytes(bodyString);
            request.ContentLength = byte1.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byte1, 0, byte1.Length);
            }


            Stream stream = request.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(stream);

            string line = sr.ReadToEnd();

            stream.Close();
            sr.Close();

            return JsonConvert.DeserializeObject<T>(line);
        }

        static T Patch<T>(string url, object body, string token)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Headers.Add("x-ads-region", "EMEA");
            request.Accept = "application/json";
            request.Method = "PATCH";
            request.ContentType = "application/json";

            string bodyString = JsonConvert.SerializeObject(body);

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] byte1 = encoding.GetBytes(bodyString);
            request.ContentLength = byte1.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byte1, 0, byte1.Length);
            }


            Stream stream = request.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(stream);

            string line = sr.ReadToEnd();

            stream.Close();
            sr.Close();

            return JsonConvert.DeserializeObject<T>(line);
        }
    }
}
