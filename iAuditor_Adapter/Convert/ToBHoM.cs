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
