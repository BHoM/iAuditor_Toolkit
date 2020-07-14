
using BH.oM.Base;
using System.Collections.Generic;
using BH.oM.HTTP;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.iAuditor
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public  Method                            ****/
        /***************************************************/

        [Description("Create a GetRequest for iAuditor")]
        [Input("apiCommand", "The iAuditor REST API command to create a GetRequest with")]
        [Input("bearerToken", "The iAuditor bearerToken (this can be acquired using Compute BearerToken with your EC3 username and password)")]
        [Input("parameters", "An optional CustomObject with properties representing parameters to create the GetRequest with (ie count, name_like, etc)")]
        [Output("GetRequest", "A GetRequest with CarbonQueryDatabase specific headers and uri")]

        public static GetRequest iAuditorRequest(string apiCommand, string bearerToken, CustomObject parameters = null)
        {
            return new BH.oM.HTTP.GetRequest
            {
                BaseUrl = "https://api.safetyculture.io/" + apiCommand,
                Headers = new Dictionary<string, object>()
                {
                    { "Authorization", "Bearer " +  bearerToken }
                },
                Parameters = parameters?.CustomData
        };
        }

        /***************************************************/
    }
}

