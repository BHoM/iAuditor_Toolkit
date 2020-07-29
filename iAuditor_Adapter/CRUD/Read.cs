using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.Adapter.iAuditor;
using BH.oM.Base;
using BH.oM.Adapter;
using BH.oM.Adapter.iAuditor;
using BH.oM.iAuditor;
using BH.oM.HTTP;
using BH.Engine.HTTP;
using BH.Engine.iAuditor;
using BH.Adapter;
using BH.Engine.Serialiser;
using BH.Engine.Reflection;

namespace BH.Adapter.iAuditor
{
    public partial class iAuditorAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/
        protected override IEnumerable<IBHoMObject> IRead(Type type, IList ids, ActionConfig actionConfig = null)
        {
            dynamic elems = null;
            iAuditorConfig config = null;

            if (actionConfig is iAuditorConfig)
                config = actionConfig as iAuditorConfig;

            //Choose what to pull out depending on the type.
            if (type == typeof(Audit))
                elems = ReadAudit(ids as dynamic, config);

            return elems;
        }

        /***************************************************/
        /**** Private specific read methods             ****/
        /***************************************************/
        private List<Audit> ReadAudit(List<string> ids = null, iAuditorConfig config = null)
        {
            //Add parameters per config
            CustomObject requestParams = new CustomObject();
            string id = null;
            string targetPath = null;
            bool includeAssetFiles = true;

            if (config != null)
            {
                id = config.Id;
                targetPath = config.AssetFilePath;
                includeAssetFiles = config.IncludeAssetFiles;
            }

            //Create GET Request
            GetRequest getRequest;
            if (id == null)
            { getRequest = BH.Engine.iAuditor.Create.iAuditorRequest("audits", m_bearerToken, requestParams); }
            else
            { getRequest = BH.Engine.iAuditor.Create.iAuditorRequest("audits/" + id, m_bearerToken); }

            string reqString = getRequest.ToUrlString();
            string response = BH.Engine.HTTP.Compute.MakeRequest(getRequest);
            List<object> responseObjs = null;
            if (response == null)
            {
                BH.Engine.Reflection.Compute.RecordWarning("No response received, check bearer token and connection.");
                return null;
            }

            //Check if the response is a valid json
            else if (response.StartsWith("{"))
            {
                response = "[" + response + "]";
                responseObjs = new List<object>() { Engine.Serialiser.Convert.FromJson(response) };
            }
            else if (response.StartsWith("["))
            {
                responseObjs = new List<object>() { Engine.Serialiser.Convert.FromJson(response) };
            }

            else
            {
                BH.Engine.Reflection.Compute.RecordWarning("Response is not a valid JSON. How'd that happen?");
                return null;
            }

            //Convert nested customObject from serialization to list of epdData objects
            List<Audit> audits = new List<Audit>();

            object auditObjects = Engine.Reflection.Query.PropertyValue(responseObjs[0], "Objects");

            IEnumerable objList = auditObjects as IEnumerable;
            if (objList != null)
            {
                foreach (CustomObject co in objList)
                {
                    Audit audit = Convert.ToAudit(co, m_bearerToken, targetPath, includeAssetFiles);
                    audits.Add(audit);
                }
            }

            return audits;
        }

    }

}
