using System.Collections.Generic;
using System.Linq;

namespace FormaHelloTerrainClient
{
    public class InternalProposalAPI
    {
        public static Element Read(string proposalId, string authcontext, string accessToken)
        {
            var elementResponse = Http.Get<Dictionary<string, Element>>("https://app.autodeskforma.eu/api/proposal/elements/" + proposalId + "/" + "?authcontext=" + authcontext, accessToken);

            return elementResponse.First().Value;
        }

    }
}
