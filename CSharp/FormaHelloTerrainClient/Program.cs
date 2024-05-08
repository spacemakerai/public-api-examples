using FormaHelloTerrainClient.PublicAPI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace FormaHelloTerrainClient
{
    internal class Program
    {

        private static string PROJECT_ID = "pro_mnpo1vppxk";
        private static string PROPOSAL_URN = "urn:adsk-forma-elements:proposal:pro_mnpo1vppxk:06739f8f-0d44-4023-b755-410f8d630f6b:1714473913972";
        private static string PROPOSAL_ID = "06739f8f-0d44-4023-b755-410f8d630f6b";

        static void Main(string[] args)
        {
            try
            {
                PublicAPIExample();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadLine();
            }
        }

        static string GetAPSToken()
        {
            var accessTokenTask = OAuthHandler.GetAccessToken();

            accessTokenTask.Wait();
            var token = accessTokenTask.Result;

            JObject jsonResponse = JObject.Parse(token);
            string accessToken = jsonResponse["access_token"].ToString();

            return accessToken;
        }

        static void PublicAPIExample()
        {
            string token = GetAPSToken();
            // string token = "YOU CAN CACHE YOUR TOKEN HERE";


            var proposal = ReadElementAPI.Read(PROPOSAL_URN, token);

            Child child = proposal.children.Find(c => c.urn.Contains(":terrain:"));
            var terrain = ReadElementAPI.Read(child.urn, token);
            var glb = TerrainAPI.Download(terrain.urn, token);

            var element = IntegrateAPI.CreateIntegrateByTriangles(PROJECT_ID, TRIANGLES, token);

            var newProposal = ProposalAPI.AddChild(proposal.urn, new Child("key-b", null, element.urn), token);

            Console.WriteLine("wop: " + token);
        }

        static void InternalAPIExample()
        {
            // This is an approach based on the non-public Forma API
            // This is just a backup solution
            string token = "COPY PASTE YOUR FORMA TOKEN HERE";

            var proposal = InternalProposalAPI.Read(PROPOSAL_ID, PROJECT_ID, token);
            Child child = proposal.children.Find(children => children.urn.Contains(":terrain:"));
            var terrain = InternalTerrainAPI.Download(child.urn, token);

        }



        static List<List<double>> CURVE = new List<List<double>>
        {
            new List<double> { 0.0, 0.0 },
            new List<double> { 10.0, 0.0 },
            new List<double> { 10.0, 10.0 },
            new List<double> { 0.0, 10.0 },
            new List<double> { 0.0, 0.0 }
        };

        static List<double> TRIANGLES = new List<double>
        {
            // Front face
            -0.5, -0.5, 0.5,   // Vertex 0
            0.5, -0.5, 0.5,    // Vertex 1
            0.5, 0.5, 0.5,     // Vertex 2

            -0.5, -0.5, 0.5,   // Vertex 0
            0.5, 0.5, 0.5,     // Vertex 2
            -0.5, 0.5, 0.5,    // Vertex 3

            // Back face
            0.5, -0.5, -0.5,   // Vertex 4
            -0.5, -0.5, -0.5,  // Vertex 5
            -0.5, 0.5, -0.5,   // Vertex 6

            0.5, -0.5, -0.5,   // Vertex 4
            -0.5, 0.5, -0.5,   // Vertex 6
            0.5, 0.5, -0.5,    // Vertex 7

            // Left face
            -0.5, -0.5, -0.5,  // Vertex 5
            -0.5, -0.5, 0.5,   // Vertex 0
            -0.5, 0.5, 0.5,    // Vertex 3

            -0.5, -0.5, -0.5,  // Vertex 5
            -0.5, 0.5, 0.5,    // Vertex 3
            -0.5, 0.5, -0.5,   // Vertex 6

            // Right face
            0.5, -0.5, 0.5,    // Vertex 1
            0.5, -0.5, -0.5,   // Vertex 4
            0.5, 0.5, -0.5,    // Vertex 7

            0.5, -0.5, 0.5,    // Vertex 1
            0.5, 0.5, -0.5,    // Vertex 7
            0.5, 0.5, 0.5,     // Vertex 2

            // Top face
            -0.5, 0.5, 0.5,    // Vertex 3
            0.5, 0.5, 0.5,     // Vertex 2
            0.5, 0.5, -0.5,    // Vertex 7

            -0.5, 0.5, 0.5,    // Vertex 3
            0.5, 0.5, -0.5,    // Vertex 7
            -0.5, 0.5, -0.5,   // Vertex 6

            // Bottom face
            -0.5, -0.5, -0.5,  // Vertex 5
            0.5, -0.5, -0.5,   // Vertex 4
            0.5, -0.5, 0.5,    // Vertex 1

            -0.5, -0.5, -0.5,  // Vertex 5
            0.5, -0.5, 0.5,    // Vertex 1
            -0.5, -0.5, 0.5    // Vertex 0
        };
    }
}
