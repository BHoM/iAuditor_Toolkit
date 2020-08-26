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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;
using BH.oM.Base;
using BH.oM.Adapters.HTTP;
using BH.Engine.Adapters.HTTP;
using BH.Engine.Reflection;
using System.Collections;
using System.IO;
using BH.Engine.Units;
using BH.oM.Adapters.iAuditor;

namespace BH.Adapter.iAuditor
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static Audit ToAudit(this CustomObject obj, string bearerToken, string targetPath, bool includeAssetFiles = true)
        {
            string projectNumber = "";
            int visitNo = 0;
            int revNo = 0;
            string client = "";
            string inspectionDate = "";
            string issueDate = "";
            string author = "";
            string jobLeader = "";
            string title = "";
            List<string> areas = new List<string>();
            string purpose = "";
            List<string> distribution = new List<string>();
            List<string> attendance = new List<string>();
            List<InstallationProgress> installProgress = new List<InstallationProgress>();
            List<Issue> issues = new List<Issue>();
            string auditID = obj.PropertyValue("audit_id")?.ToString() ?? "";

            if (obj.PropertyValue("items") != null)
            {
                List<object> items = obj.PropertyValue("items") as List<object>;
                for (int i = 0; i < items.Count(); i++)
                {
                    if (items[i].PropertyValue("label").ToString() == "Job leader")
                    {
                        jobLeader = items[i].PropertyValue("responses.text")?.ToString() ?? "";
                    }
                    else if (items[i].PropertyValue("label").ToString() == "Purpose of visit")
                    {
                        purpose = items[i].PropertyValue("responses.text")?.ToString() ?? "";
                    }
                    else if (items[i].PropertyValue("label").ToString() == "Distribution")
                    {
                        string val = items[i].PropertyValue("responses.text").ToString();
                        List<string> valList = val.Split(new char[] { '\n', ',' }).ToList();
                        foreach (string entry in valList)
                        {
                            distribution.Add(entry.Trim());
                        }
                    }
                    else if (items[i].PropertyValue("label").ToString() == "Attendance")
                    {
                        string val = items[i].PropertyValue("responses.text").ToString();
                        List<string> valList = val.Split(new char[] { '\n', ',' }).ToList();
                        foreach (string entry in valList)
                        {
                            attendance.Add(entry.Trim());
                        }
                    }
                    else if (items[i].PropertyValue("label").ToString() == "Areas inspected")
                    {
                        string val = items[i].PropertyValue("responses.text").ToString();
                        List<string> valList = val.Split(new char[] { '\n', ',' }).ToList();
                        foreach (string entry in valList)
                        {
                            areas.Add(entry.Trim());
                        }
                    }
                    else if (items[i].PropertyValue("label").ToString() == "Audit Title")
                    {
                        title = (items[i].PropertyValue("responses.text")?.ToString() ?? "");
                    }
                    else if (includeAssetFiles == true && items[i].PropertyValue("type").ToString() == "media" && Query.PropertyNames(items[i]).Contains("media"))
                    {
                        List<object> mediaObjs = items[i].PropertyValue("media") as List<object>;
                        foreach (object mediaObj in mediaObjs)
                        {
                            GetRequest getMediaRequest = BH.Engine.Adapters.iAuditor.Create.iAuditorRequest("audits/" + auditID + @"/media/" + mediaObj.PropertyValue("media_id").ToString(), bearerToken);
                            string reqString = getMediaRequest.ToUrlString();
                            byte[] response = BH.Engine.Adapters.HTTP.Compute.MakeRequestBinary(getMediaRequest);
                            File.WriteAllBytes(targetPath + @"/" + mediaObj.PropertyValue("media_id").ToString() + "." + mediaObj.PropertyValue("file_ext").ToString(), response);
                        }
                    }
                }
            }

            if (obj.PropertyValue("header_items") != null)
            {
                List<object> items = obj.PropertyValue("header_items") as List<object>;
                for (int i = 0; i < items.Count(); i++)
                {
                    if (items[i].PropertyValue("label").ToString() == "Job Number")
                    {
                        projectNumber = (items[i].PropertyValue("responses.text")?.ToString() ?? "");
                    }
                    else if (items[i].PropertyValue("label").ToString() == "Audit Title")
                    {
                        title = (items[i].PropertyValue("responses.text")?.ToString() ?? "");
                    }
                    else if (items[i].PropertyValue("label").ToString() == "Site Visit No.")
                    {
                        int.TryParse((items[i].PropertyValue("responses.text")?.ToString() ?? ""), out visitNo);
                    }
                    else if (items[i].PropertyValue("label").ToString() == "Client")
                    {
                        client = (items[i].PropertyValue("responses.text")?.ToString() ?? "");
                    }
                    else if (items[i].PropertyValue("label").ToString() == "Author")
                    {
                        author = (items[i].PropertyValue("responses.text")?.ToString() ?? "");
                    }
                    else if (items[i].PropertyValue("label").ToString() == "Date of issue")
                    {
                        issueDate = (items[i].PropertyValue("responses.datetime")?.ToString() ?? "");
                    }
                    else if (items[i].PropertyValue("label").ToString() == "Date of inspection")
                    {
                        inspectionDate = (items[i].PropertyValue("responses.datetime")?.ToString() ?? "");
                    }
                    else if (items[i].PropertyValue("label").ToString() == "Revision no.")
                    {
                        int.TryParse((items[i].PropertyValue("responses.text")?.ToString() ?? ""), out revNo);
                    }
                }
            }

            installProgress = obj.ToInstallationProgressObjects();
            issues = obj.ToIssues(targetPath, includeAssetFiles);

            Audit audit = new Audit
            {
                Title = title,
                Filename = title,
                AuditID = auditID,
                SiteVisitNumber = visitNo,
                Client = client,
                Author = author,
                ProjectNumber = projectNumber,
                JobLeader = jobLeader,
                Name = obj.PropertyValue("audit_data.name")?.ToString() ?? "",
                RevisionNumber = revNo,
                IssueDate = issueDate,
                InspectionDate = inspectionDate,
                Distribution = distribution,
                Attendance = attendance,
                VisitPurpose = purpose,
                AreasInspected = areas,
                InstallationProgressObjects = installProgress,
                Issues = issues,
                Score = obj.PropertyValue("audit_data.score_percentage")?.ToString() ?? "",
            };

            return audit;
        }

        /***************************************************/

        public static List<InstallationProgress> ToInstallationProgressObjects(this CustomObject obj)
        {
            List<InstallationProgress> installProgList = new List<InstallationProgress>();

            if (obj.PropertyValue("items") != null)
            {
                List<object> items = obj.PropertyValue("items") as List<object>;
                for (int i = 0; i < items.Count(); i++)
                {
                    if (items[i].PropertyValue("label").ToString() == @"Elevation / area" && items[i].PropertyNames().Contains("responses"))
                    {
                        InstallationProgress installObject = ToInstallationProgress(items[i] as CustomObject, obj);
                        installProgList.Add(installObject);
                    }
                }
            }
            return installProgList;
        }

        /***************************************************/

        public static InstallationProgress ToInstallationProgress(this CustomObject obj, CustomObject auditCustomObject)
        {
            string generalStatus = "";
            if (auditCustomObject.PropertyValue("items") != null)
            {
                List<object> items = auditCustomObject.PropertyValue("items") as List<object>;
                for (int i = 0; i < items.Count(); i++)
                {
                    if (items[i].PropertyValue("label").ToString() == "General status")
                    {
                        generalStatus = items[i].PropertyValue("responses.text")?.ToString() ?? "";
                    }
                } 
            }

            //TODO media from install progress objects dependent on iAuditor Template Fix

            string area = obj.PropertyValue("responses.text")?.ToString() ?? "";
            InstallationProgress installProgObj = new InstallationProgress
            {
                Status = generalStatus,
                Area = area
            };
            return installProgObj;
        }

        /***************************************************/

        public static List<Issue> ToIssues(this CustomObject obj, string targetPath, bool includeAssetFiles = true)
        {
            List<Issue> issues = new List<Issue>();
            List<string> commentIDs = new List<string>();

            if (obj.PropertyValue("items") != null)
            {
                List<object> items = obj.PropertyValue("items") as List<object>;

                // collect ids of each issue from iAuditor Site Comment Heading category
                for (int i = 0; i < items.Count(); i++)
                {
                    if (items[i].PropertyValue("label").ToString() == @"Site comment #")
                    {
                        List<object> commentIDList = items[i].PropertyValue("children") as List<object>;
                        commentIDList.ForEach(x => commentIDs.Add(x.ToString()));
                    }
                }
                for (int i = 0; i < items.Count(); i++)
                {
                    if (commentIDs.Contains(items[i].PropertyValue("item_id").ToString()) && items[i].PropertyValue("type").ToString() == "element")
                    {
                        Issue issue = ToIssue(items[i] as CustomObject, obj, targetPath, includeAssetFiles);
                        issues.Add(issue);
                    }
                }
            }
            return issues;
        }

        /***************************************************/

        public static Issue ToIssue(this CustomObject obj, CustomObject auditCustomObject, string targetPath, bool includeAssetFiles = true)
        {
            List<object> commentElems = new List<object>();
            List<string> commentElemIDs = new List<string>();
            List<object> commentIDList = obj.PropertyValue("children") as List<object>;
            commentIDList.ForEach(x => commentElemIDs.Add(x.ToString()));
            List<object> items = auditCustomObject.PropertyValue("items") as List<object>;
            List<string> media = new List<string>();
            string priority = "";
            string status = "";
            string assign = "";
            string description = "";
            for (int i = 0; i < items.Count(); i++)
            {
                if (commentElemIDs.Contains(items[i].PropertyValue("item_id").ToString()))
                {
                    commentElems.Add(items[i]);
                }
            }

            for (int i = 0; i < commentElems.Count(); i++)
            {
                if (commentElems[i].PropertyValue("label").ToString() == "Priority")
                {
                    List<object> vals = commentElems[i].PropertyValue("responses.selected") as List<object>;
                    priority = vals[0].PropertyValue("label").ToString();
                }
                else if (commentElems[i].PropertyValue("label").ToString() == "Status")
                {
                    List<object> vals = commentElems[i].PropertyValue("responses.selected") as List<object>;
                    status = vals[0].PropertyValue("label").ToString();
                }
                else if (commentElems[i].PropertyValue("label").ToString() == "Assign")
                {
                    List<object> vals = commentElems[i].PropertyValue("responses.selected") as List<object>;
                    assign = vals[0].PropertyValue("label").ToString();
                }
                else if (commentElems[i].PropertyValue("label").ToString().Contains("description"))
                {
                    description = (commentElems[i].PropertyValue("responses.text")?.ToString() ?? "");
                }
                else if (commentElems[i].PropertyValue("type").ToString().Contains("media"))
                {
                    List<object> mediaObjs = commentElems[i].PropertyValue("media") as List<object>;
                    List<CustomObject> mediaCos = mediaObjs.Select(x => (CustomObject)x).ToList();
                    mediaCos.ForEach(x => media.Add(ToMedia(x, auditCustomObject, targetPath, includeAssetFiles)));
                }
            }

            Issue issue = new Issue
            {
                Description = description,
                Priority = priority,
                Status = status,
                Assign = assign,
                Media = media
            };
                return issue;
        }

        /***************************************************/

        public static string ToMedia(this CustomObject obj, CustomObject auditCustomObject, string targetPath, bool includeAssetFiles = true)
        {
            string media;
            string mediaID = obj.PropertyValue("media_id") as string;
            string mediaExtension = obj.PropertyValue("file_ext") as string;

            media = mediaID + "." + mediaExtension;

            return media;
        }

        /***************************************************/
    }
}
