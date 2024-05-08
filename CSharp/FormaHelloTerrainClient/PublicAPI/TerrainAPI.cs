using System;
using System.IO.Compression;
using System.IO;
using System.Net;

namespace FormaHelloTerrainClient
{
    public class TerrainAPI
    {
        public static SharpGLTF.Schema2.ModelRoot Download(string urn, string token)
        {
            string[] parts = urn.Split(':');
            string authcontext = parts[3];
            string elementId = parts[4];
            string revision = parts[5];

            string url = "https://developer.api.autodesk.com/forma/terrain/v1alpha/terrains/" + elementId + "/revisions/" + revision + "/download?authcontext=" + authcontext;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Headers.Add("x-ads-region", "EMEA");
            request.Accept = "application/json";
            request.Method = "GET";

            var response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();

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
