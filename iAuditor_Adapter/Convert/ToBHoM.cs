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
                Score = obj.PropertyValue("audit_data.score_percentage")?.ToString() ?? "",
            };

            return audit;
        }

        /***************************************************/

    }
}
