using Newtonsoft.Json;
using System.Collections.Generic;

namespace FormaHelloTerrainClient
{

    public class ElementResponse
    {
        [JsonProperty]
        public Element element;
        [JsonProperty]
        public Dictionary<string, Element> elements;
    }
    public class Element
    {
        [JsonProperty]
        public string urn;

        [JsonProperty]
        public Dictionary<string, object> metadata;

        [JsonProperty]
        public Dictionary<string, object> properties;

        [JsonProperty]
        public List<Child> children;

        [JsonProperty]
        public Dictionary<string, Representation> representations;

        [JsonConstructor]
        public Element(string urn, Dictionary<string, object> metadata, Dictionary<string, object> properties, Dictionary<string, Representation> representations, List<Child> children)
        {
            this.urn = urn;
            this.metadata = metadata;
            this.properties = properties;
            this.representations = representations;
            this.children = children;
        }
    }

    public class Child
    {
        [JsonProperty]
        public string key;
        [JsonProperty]
        public double[] transform;
        [JsonProperty]
        public string urn;

        [JsonConstructor]
        public Child(string key, double[] transform, string urn)
        {
            this.key=key;
            this.transform=transform;
            this.urn=urn;
        }
    }

    public class Representation
    {
        [JsonProperty]
        public string url;

        [JsonConstructor]
        public Representation(string url)
        {
            this.url=url;
        }
    }

}
