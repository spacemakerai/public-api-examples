namespace FormaHelloTerrainClient
{
    public class ReadElementAPI
    {
        public static Element Read(string urn, string accessToken) {
            var authcontext = urn.Split(':')[3];

            var response = Http.Get<ElementResponse>("https://developer.api.autodesk.com/forma/element-service/v1alpha/elements/" + urn + "?authcontext=" + authcontext, accessToken);

            return response.element;
        }
    }
}
