using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpGLTF.Transforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FormaHelloTerrainClient.PublicAPI
{
    public class ProposalAPI
    {

        public static Element Duplicate(string urn, string token)
        {
            return null;
        }
        public static Element AddChild(string urn, Child child, string token)
        {
            Element proposal = ReadElementAPI.Read(urn, token);

            var flags = proposal.properties["flags"] as JObject;
            var name = proposal.properties["name"];


            Child terrainRef = proposal.children.Find(p => { return p.urn.StartsWith("urn:adsk-forma-elements:terrain:"); });
            Child baseRef = proposal.children.Find(p => { return flags[p.key] != null && (bool)flags[p.key]["base"]; });
            List<Child> children = proposal.children.FindAll(p => { return p != terrainRef && p != baseRef; });

            ProposalCreate create = new ProposalCreate(
                proposal.properties["name"] as string + "1",
                terrainRef,
                baseRef,
                children.Concat(new Child[] { child }).ToArray()
            );

            var parts = urn.Split(':');
            string authcontext = parts[3];
            string elementId = parts[4];
            string revision = parts[5];


            var prop = Http.Put<Element>("https://developer.api.autodesk.com/forma/proposal/v1alpha/proposals/" + elementId + "/revisions/" + revision + "?authcontext=" + authcontext, create, token);

            return ReadElementAPI.Read(prop.urn, token);
        }

        public static Element RemoveChild(string urn, string key, string token)
        { 
            return null; 
        }

        public static Element ReplaceChild(string urn, string key, Child child, string token)
        { 
            return null; 
        }


        private class ProposalCreate
        {
            [JsonProperty]
            public string name;
            [JsonProperty]
            public Child terrain;
            [JsonProperty("base")]
            public Child baseElement;
            [JsonProperty]
            public Child[] children;

            [JsonConstructor]
            public ProposalCreate(string name, Child terrain, Child baseElement, Child[] children)
            {
                this.name = name;
                this.terrain = terrain;
                this.baseElement = baseElement;
                this.children = children;
            }
        }
    }
}
