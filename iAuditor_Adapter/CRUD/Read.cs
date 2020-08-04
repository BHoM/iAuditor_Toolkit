/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

 using System;
using System.IO;
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
            bool includeAssetFiles = false;

            if (config != null)
            {
                id = config.Id;
                includeAssetFiles = config.IncludeAssetFiles;
                targetPath = config.AssetFilePath;
                if (includeAssetFiles == true)
                {
                    //Filepath handling to create or direct to provided relevant directory 
                    try
                    {                       
                        if (File.Exists(targetPath))
                        {
                            targetPath = Path.GetFullPath(targetPath);
                            FileAttributes attr = File.GetAttributes(targetPath);
                            if (attr.HasFlag(FileAttributes.Directory) == false)
                                targetPath = Path.GetDirectoryName(targetPath);    
                        }
                        Directory.CreateDirectory(targetPath);
                    }
                    catch
                    {
                        targetPath = @"C:\BHoM\iAuditorAssets";
                        Directory.CreateDirectory(targetPath);
                        BH.Engine.Reflection.Compute.RecordWarning("Path is invalid, does not exist, or was not provided. Using " + targetPath + " as target path for assets. The target path must be an already existing folder in our environment.");
                    }                   
                }
            }

            //Create GET Request
            GetRequest getRequest;
            if (id == null)
            {
                Engine.Reflection.Compute.RecordError("No audit ID provided. Please provide an audit ID using an iAuditorConfig ActionConfig.");
                return new List<BH.oM.iAuditor.Audit>();
            }
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
