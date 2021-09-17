/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
using BH.oM.Geometry;
using BH.Engine.Reflection;
using System.Collections;
using System.IO;
using BH.Engine.Units;
using BH.oM.Inspection;
using BH.oM.Adapters.iAuditor;

namespace BH.Adapter.iAuditor
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static Audit ToAudit(this CustomObject obj, string targetPath, string bearerToken, bool includeAssetFiles = true)
        {
            List<BHoMObject> auditAndIssues = new List<BHoMObject>();
            string projectNumber = "";
            int visitNo = 0;
            int revNo = 0;
            string client = "";
            string inspectionDateString = "";
            DateTime inspectionDate = new DateTime();
            DateTime inspectionDateUtc = new DateTime();
            string issueDateString = "";
            DateTime issueDate = new DateTime();
            DateTime issueDateUtc = new DateTime();
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
                        issueDateString = (items[i].PropertyValue("responses.datetime")?.ToString() ?? "");
                        DateTime.TryParse(issueDateString, out issueDate);
                        issueDateUtc = issueDate;
                    }
                    else if (items[i].PropertyValue("label").ToString() == "Date of inspection")
                    {
                        inspectionDateString = (items[i].PropertyValue("responses.datetime")?.ToString() ?? "");
                        DateTime.TryParse(issueDateString, out inspectionDate);
                        inspectionDateUtc = inspectionDate;
                    }
                    else if (items[i].PropertyValue("label").ToString() == "Revision no.")
                    {
                        int.TryParse((items[i].PropertyValue("responses.text")?.ToString() ?? ""), out revNo);
                    }
                }
            }

            installProgress = obj.ToInstallationProgressObjects(includeAssetFiles, targetPath, bearerToken);
            issues = obj.ToIssues(targetPath, bearerToken, false);
            List<string> issueIDs = new List<string>();
            foreach (Issue issue in issues)
            {
                issueIDs.Add(issue.IssueNumber);
            }

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
                InspectionDate = inspectionDate,
                InspectionDateUtc = inspectionDateUtc,
                IssueDate = issueDate,
                IssueDateUtc = issueDateUtc,
                Distribution = distribution,
                Attendance = attendance,
                VisitPurpose = purpose,
                AreasInspected = areas,
                InstallationProgressObjects = installProgress,
                IssueNumbers = issueIDs,
                Score = obj.PropertyValue("audit_data.score_percentage")?.ToString() ?? "",
            };

            return audit;
        }

        /***************************************************/

        public static List<InstallationProgress> ToInstallationProgressObjects(this CustomObject auditObj, bool includeAssetFiles, string targetPath, string bearerToken)
        {
            List<InstallationProgress> installProgList = new List<InstallationProgress>();

            if (auditObj.PropertyValue("items") != null)
            {
                List<object> items = auditObj.PropertyValue("items") as List<object>;
                for (int i = 0; i < items.Count(); i++)
                {
                    if (items[i].PropertyValue("label").ToString() == @"Elevation / area" && items[i].PropertyNames().Contains("responses"))
                    {
                        InstallationProgress installObject = ToInstallationProgress(items[i] as CustomObject, auditObj, i, includeAssetFiles, targetPath, bearerToken ); // item count used to get adjacent media entry that follows,
                        installProgList.Add(installObject);
                    }
                }
            }
            return installProgList;
        }

        /***************************************************/

        public static InstallationProgress ToInstallationProgress(this CustomObject obj, CustomObject auditCustomObject, int itemNumber, bool includeAssetFiles, string targetPath, string bearerToken)
        {
            string generalStatus = "";
            List<string> media = new List<string>();
            string auditID = auditCustomObject.PropertyValue("audit_id")?.ToString() ?? "";

            // Get General status from audit, and media label from entry following installation progress description entry
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
                if (items[itemNumber + 1].PropertyValue("type").ToString().Contains("media") && Query.PropertyNames(items[itemNumber + 1]).Contains("media"))
                {
                    List<object> mediaObjs = items[itemNumber + 1].PropertyValue("media") as List<object>;
                    if (mediaObjs != null)
                    {
                        List<CustomObject> mediaCos = mediaObjs.Select(x => (CustomObject)x).ToList();
                        mediaCos.ForEach(x => media.Add(ToMedia(x, auditID, targetPath, includeAssetFiles, bearerToken)));
                    }
                }
            }

            string area = obj.PropertyValue("responses.text")?.ToString() ?? "";
            InstallationProgress installProgObj = new InstallationProgress
            {
                Status = generalStatus,
                Area = area,
                Media = media
            };
            return installProgObj;
        }

        /***************************************************/

        public static List<Issue> ToIssues(this CustomObject obj, string targetPath, string bearerToken, bool includeAssetFiles = true)
        {
            List<Issue> issues = new List<Issue>();
            List<string> commentIDs = new List<string>();
            int issueCounter = 1;

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
                        Issue issue = ToIssue(items[i] as CustomObject, obj, issueCounter, targetPath, includeAssetFiles, bearerToken);
                        issues.Add(issue);
                        issueCounter += 1;
                    }
                }
            }
            return issues;
        }

        /***************************************************/

        public static Issue ToIssue(this CustomObject obj, CustomObject auditCustomObject, int issueNo, string targetPath, bool includeAssetFiles, string bearerToken)
        {
            List<object> commentElems = new List<object>();
            List<string> commentElemIDs = new List<string>();
            List<object> commentIDList = obj.PropertyValue("children") as List<object>;
            commentIDList.ForEach(x => commentElemIDs.Add(x.ToString()));
            List<object> items = auditCustomObject.PropertyValue("items") as List<object>;
            string auditID = auditCustomObject.PropertyValue("audit_id")?.ToString() ?? "";

            List<string> media = new List<string>();
            string priority = "";
            string status = "";
            string type = "";
            List<string> assign = new List<string>();
            string description = "";
            string issueDateString = "";
            string visitNo = "00";
            DateTime issueDate = new DateTime();

            // Get audit number to add to issue number for issue tracking and audit issued date
            if (auditCustomObject.PropertyValue("header_items") != null)
            {
                List<object> auditItems = auditCustomObject.PropertyValue("header_items") as List<object>;
                for (int i = 0; i < auditItems.Count(); i++)
                {
                    if (auditItems[i].PropertyValue("label").ToString() == "Site Visit No.")
                    {
                        visitNo = auditItems[i].PropertyValue("responses.text")?.ToString();
                    }
                    else if (auditItems[i].PropertyValue("label").ToString() == "Date of issue")
                    {
                        issueDateString = (auditItems[i].PropertyValue("responses.datetime")?.ToString() ?? "");
                        DateTime.TryParse(issueDateString, out issueDate);
                    }
                }
            }
            string issueNumber = visitNo.PadLeft(2, '0') + "." + issueNo.ToString().PadLeft(2, '0');

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
                    assign.Add(vals[0].PropertyValue("label").ToString());
                }
                else if (commentElems[i].PropertyValue("label").ToString() == "Type")
                {
                    List<object> vals = commentElems[i].PropertyValue("responses.selected") as List<object>;
                    type = vals[0].PropertyValue("label").ToString();
                }
                else if (commentElems[i].PropertyValue("label").ToString().Contains("description"))
                {
                    description = (commentElems[i].PropertyValue("responses.text")?.ToString() ?? "");
                }
                else if (commentElems[i].PropertyValue("type").ToString().Contains("media") && Query.PropertyNames(commentElems[i]).Contains("media"))
                {
                    List<object> mediaObjs = commentElems[i].PropertyValue("media") as List<object>;
                    if (mediaObjs != null)
                    {
                        List<CustomObject> mediaCos = mediaObjs.Select(x => (CustomObject)x).ToList();
                        mediaCos.ForEach(x => media.Add(ToMedia(x, auditID, targetPath, includeAssetFiles, bearerToken)));
                    }                  
                }
            }

            Issue issue = new Issue
            {
                AuditID = auditID,
                Description = description,
                DateCreated = issueDate,
                IssueNumber = issueNumber,
                Priority = priority,
                Status = status,
                Type = type,
                Assign = assign,
                Media = media,
                Name = issueNumber
            };
                return issue;
        }

        /***************************************************/

        public static string ToMedia(this CustomObject obj, string auditID, string targetPath, bool includeAssetFiles, string bearerToken)
        {
            string media;
            string mediaID = obj.PropertyValue("media_id") as string;
            string mediaExtension = obj.PropertyValue("file_ext") as string;

            if (includeAssetFiles)
            {
                    GetRequest getMediaRequest = BH.Engine.Adapters.iAuditor.Create.IAuditorRequest("audits/" + auditID + @"/media/" + mediaID, bearerToken);
                    string reqString = getMediaRequest.ToUrlString();
                    byte[] response = BH.Engine.Adapters.HTTP.Compute.MakeRequestBinary(getMediaRequest);
                    File.WriteAllBytes(targetPath + @"/" + mediaID + "." + mediaExtension, response);
            }

            media = mediaID + "." + mediaExtension;

            return media;
        }

        /***************************************************/
    }
}

