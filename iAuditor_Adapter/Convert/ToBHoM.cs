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
using BH.Engine.Reflection;
using System.Collections;
using BH.Engine.Units;
using BH.oM.iAuditor;

namespace BH.Adapter.iAuditor
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static Audit ToAudit(this CustomObject obj)
        {
            List<string> labels = new List<string>();
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
            List<Comment> comments = new List<Comment>();

            if (obj.PropertyValue("items") != null)
            {
                List<object> items = obj.PropertyValue("items") as List<object>;
                for (int i = 0; i < items.Count(); i++)
                {
                    labels.Add(items[i].PropertyValue("label")?.ToString() ?? "");
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
            comments = obj.ToComments();

            Audit audit = new Audit
            {
                Title = title,
                Filename = title,
                TemplateID = obj.PropertyValue("template_id")?.ToString() ?? "",
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
                Comments = comments,
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

            string area = obj.PropertyValue("responses.text")?.ToString() ?? "";
            InstallationProgress installProgObj = new InstallationProgress
            {
                Status = generalStatus,
                Area = area,
            };
            return installProgObj;
        }

        /***************************************************/

        public static List<Comment> ToComments(this CustomObject obj)
        {
            List<Comment> comments = new List<Comment>();
            List<string> commentIDs = new List<string>();

            if (obj.PropertyValue("items") != null)
            {
                List<object> items = obj.PropertyValue("items") as List<object>;
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
                        Comment comment = ToComment(items[i] as CustomObject, obj);
                        comments.Add(comment);
                    }
                }
            }
            return comments;
        }

        /***************************************************/

        public static Comment ToComment(this CustomObject obj, CustomObject auditCustomObject)
        {
            List<object> commentElems = new List<object>();
            List<string> commentElemIDs = new List<string>();
            List<object> commentIDList = obj.PropertyValue("children") as List<object>;
            commentIDList.ForEach(x => commentElemIDs.Add(x.ToString()));
            List<object> items = auditCustomObject.PropertyValue("items") as List<object>;
            string priority = "";
            string status = "";
            string assign = "";
            string type = "";
            string location = "";
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
            }

            Comment comment = new Comment
            {
                Description = description,
                Priority = priority,
                Status = status,
                Assign = assign,
            };
                return comment;
        }

        /***************************************************/
    }
}
