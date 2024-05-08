using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace FormaHelloTerrainClient
{
    public class InternalTerrainAPI
    {
        public static SharpGLTF.Schema2.ModelRoot Download(string urn, string accessToken)
        {
            string[] parts = urn.Split(':');
            string authcontext = parts[3];
            string elementId = parts[4];
            string revision = parts[5];

           // var output = Http.Get<string>("https://developer.api.autodesk.com/forma/terrain/v1alpha/terrains/" + elementId + "/revisions/ " + revision + "/download?authcontext=" + authcontext, accessToken);

            var elementResponse = Http.Get<Dictionary<string, Element>>("https://app.autodeskforma.eu/api/terrain/elements/" + elementId + "/revisions/" + revision + "?authcontext=" + authcontext, accessToken);
            var element = elementResponse.First().Value;
            var volumeMesh = element.representations["volumeMesh"];

            var url = "https://app.autodeskforma.eu" + volumeMesh.url;
            return DownloadGlb(url, accessToken);
        }

        private static SharpGLTF.Schema2.ModelRoot DownloadGlb(string url, string token)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Headers.Add("x-ads-region", "EMEA");
            request.Accept = "application/json";
            request.Method = "GET";


            Stream stream = request.GetResponse().GetResponseStream();
            
           
            using (var memoryStream = new MemoryStream())
            {

                stream.CopyTo(memoryStream);
                var bytes = memoryStream.ToArray();
                var from = new MemoryStream(bytes);
                var to = new MemoryStream();
                var gZipStream = new GZipStream(from, CompressionMode.Decompress);
                gZipStream.CopyTo(to);



                var settings = new SharpGLTF.Schema2.ReadSettings()
                {
                    Validation = SharpGLTF.Validation.ValidationMode.TryFix
                };

                return SharpGLTF.Schema2.ModelRoot.ParseGLB(new ArraySegment<byte>(to.ToArray()), settings);

            }
        }
    }


}
