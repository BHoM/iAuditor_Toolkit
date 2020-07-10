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
            if (obj.PropertyValue("items") != null)
            {
                List<object> items = obj.PropertyValue("items") as List<object>;
                for (int i = 0; i<items.Count(); i++)
                {
                    labels.Add(items[i].PropertyValue("label")?.ToString() ?? "");
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
                }
            }

            Audit audit = new Audit
            {
                ProjectNumber = projectNumber,
                TemplateID = obj.PropertyValue("template_id")?.ToString() ?? "",
                Author = obj.PropertyValue("template_data.authorship.author")?.ToString() ?? "",
                Name = obj.PropertyValue("audit_data.name")?.ToString() ?? "",
                Labels = labels,
                Score = obj.PropertyValue("audit_data.score_percentage")?.ToString() ?? "",
            };

            return audit;
        }

        /***************************************************/

    }
}
