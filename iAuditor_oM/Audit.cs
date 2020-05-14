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


using System.ComponentModel;
using System.Collections.Generic;
using BH.oM.Base;


namespace BH.oM.iAuditor
{

    [Description("Audit from iAuditor")]
    public class Audit : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Title of audit")]
        public virtual string Title { get; set; }

        [Description("Filename of audit")]
        public virtual string Filename { get; set; }

        [Description("Site Visit Number of audit")]
        public virtual int SiteVisitNumber { get; set; }

        [Description("Client for which the audit is being recorded")]
        public virtual string Client { get; set; }

        [Description("Revision Number of audit")]
        public virtual int RevisionNumber { get; set; }

        [Description("Date of inspection for audit (YYYYDDMM date format)")]
        public virtual int InspectionDate { get; set; }

        [Description("Date of issue corresponding to audit (YYYYDDMM date format)")]
        public virtual int IssueDate { get; set; }

        [Description("Creator of audit")]
        public virtual string Author { get; set; }

        [Description("Project Number of audit")]
        public virtual string ProjectNumber { get; set; }

        [Description("Job leader of audit")]
        public virtual string JobLeader { get; set; }

        [Description("List of... Please provided a description tag for Distribution")]
        public virtual List<string> Distribution { get; set; }

        [Description("List of persons in attendance during audit")]
        public virtual List<string> Attendance { get; set; }

        [Description("Reason(s) for visit and audit")]
        public virtual List<string> VisitPurpose { get; set; }

        [Description("List of areas inspected throughout the audit")]
        public virtual List<string> AreasInspected { get; set; }

        [Description("General status update to be documented within the audit")]
        public virtual string Status { get; set; }

        [Description("Please provided a description tag for Elevation")]
        public virtual string Elevation { get; set; } // is this referencing the specific areas audited, would this not correspond to the AreasInspected prop?

        [Description("ID of Audit")]
        public virtual string TemplateID { get; set; }

        [Description("Labels available from the Audit")]
        public virtual List<string> Labels { get; set; }

        [Description("Score as a percentage")]
        public virtual string Score { get; set; }

        /***************************************************/
    }
}

