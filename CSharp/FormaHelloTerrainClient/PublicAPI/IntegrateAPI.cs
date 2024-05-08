using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FormaHelloTerrainClient.PublicAPI
{
    public class IntegrateAPI
    {



        public static Element CreateIntegrateByCurve(string projectId, List<List<List<double>>> positions, string token)
        {
            Dictionary<string, IntegrateCreateElement> elements = new Dictionary<string, IntegrateCreateElement>();
            elements.Add("root", new IntegrateCreateElement(
                        "root",
                        new IntegrateCreateProperties(
                            "generic",
                            "<3rd-party>",
                            new IntegrateCreateGeometryInlineGeojson(
                                "Inline",
                                "GeoJSON",
                                new FeatureCollection(new Feature[]
                                {
                                    new Feature(new Polygon(positions))
                                })
                            )
                            )));

            IntegrateCreate create = new IntegrateCreate(
                "root",
                elements
            );

            var integrate = Http.Post<Integrate>("https://developer.api.autodesk.com/forma/integrate/v1alpha/elements?authcontext=" + projectId, create, token);

            return ReadElementAPI.Read(integrate.urn, token);

        }


        public static Element CreateIntegrateByTriangles(string projectId, List<double> verts, string token)
        {
            List<int> faces = new List<int>(verts.Count / 3);
            for (int i = 0; i<verts.Count / 3; i++)
            { faces.Add(i); }

            Dictionary<string, IntegrateCreateElement> elements = new Dictionary<string, IntegrateCreateElement>();
            elements.Add("root", new IntegrateCreateElement(
                        "root",
                        new IntegrateCreateProperties(
                            "generic",
                            "<my-3rd-party>",
                            new IntegrateCreateGeometryInlineMesh(
                                verts,
                                faces
                            )
                            )));

            IntegrateCreate create = new IntegrateCreate(
                "root",
                elements
            );

            var integrate = Http.Post<Integrate>("https://developer.api.autodesk.com/forma/integrate/v1alpha/elements?authcontext=" + projectId, create, token);

            return ReadElementAPI.Read(integrate.urn, token);

        }

        private class FeatureCollection
        {
            [JsonProperty]
            public string type = "FeatureCollection";
            [JsonProperty]
            public Feature[] features;

            [JsonConstructor]
            public FeatureCollection(Feature[] features)
            {
                this.features = features;
            }
        }

        private class Feature
        {
            [JsonProperty]
            public string type = "Feature";
            [JsonProperty]
            public Polygon geometry;
            [JsonProperty]
            public Dictionary<string, string> properties = new Dictionary<string, string>();

            [JsonConstructor]
            public Feature(Polygon polygon)
            {
                this.geometry = polygon;
            }
        }

        private class Polygon
        {
            [JsonProperty]
            public string type = "Polygon";
            [JsonProperty]
            public List<List<List<double>>> coordinates;

            [JsonConstructor]
            public Polygon(List<List<List<double>>> coordinates)
            {
                this.coordinates = coordinates;
            }
        }


        private class IntegrateCreate
        {
            [JsonProperty]
            public string rootElement;
            [JsonProperty]
            public Dictionary<string, IntegrateCreateElement> elements;

            [JsonConstructor]
            public IntegrateCreate(string rootElement, Dictionary<string, IntegrateCreateElement> elements)
            {
                this.rootElement = rootElement;
                this.elements = elements;
            }
        }

        private class IntegrateCreateElement
        {
            [JsonProperty]
            public string id;
            [JsonProperty]
            public IntegrateCreateProperties properties;

            [JsonConstructor]
            public IntegrateCreateElement(string id, IntegrateCreateProperties properties)
            {
                this.id = id;
                this.properties = properties;
            }
        }

        private class IntegrateCreateProperties
        {
            [JsonProperty]
            public string category;
            [JsonProperty]
            public string elementProvider;
            [JsonProperty]
            public IntegrateCreateGeometry geometry;

            [JsonConstructor]
            public IntegrateCreateProperties(string category, string elementProvider, IntegrateCreateGeometry geometry)
            {
                this.category = category;
                this.elementProvider = elementProvider;
                this.geometry = geometry;
            }
        }

        private abstract class IntegrateCreateGeometry { }

        private class IntegrateCreateGeometryInlineMesh : IntegrateCreateGeometry
        {
            [JsonProperty]
            public string type = "Inline";
            [JsonProperty]
            public string format = "Mesh";
            [JsonProperty]
            public Boolean doubleSided;
            [JsonProperty]
            public List<double> verts;
            [JsonProperty]
            public List<int> faces;

            [JsonConstructor]
            public IntegrateCreateGeometryInlineMesh(List<double> verts, List<int> faces, Boolean doubleSided = false)
            {
                this.verts = verts;
                this.faces = faces;
                this.doubleSided = doubleSided;

            }
        }

        private class IntegrateCreateGeometryInlineGeojson : IntegrateCreateGeometry
        {
            [JsonProperty]
            public string type;
            [JsonProperty]
            public string format;
            [JsonProperty]
            public FeatureCollection geoJson;

            [JsonConstructor]
            public IntegrateCreateGeometryInlineGeojson(string type, string format, FeatureCollection geoJson)
            {
                this.type   = type;
                this.format = format;
                this.geoJson = geoJson;
            }
        }

        private class Integrate
        {
            [JsonProperty]
            public string urn;

            [JsonConstructor]
            public Integrate(string urn)
            {
                this.urn = urn;
            }
        }

    }
}
