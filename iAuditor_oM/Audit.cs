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

        [Description("ID of Audit")]
        public virtual string TemplateID { get; set; }

        [Description("Creator of audit")]
        public virtual string Author { get; set; }

        [Description("Project Number")]
        public virtual int ProjectNumber { get; set; }

        [Description("Labels available from the Audit")]
        public virtual List<string> Labels { get; set; }

        [Description("Score as a percentage")]
        public virtual string Score { get; set; }

        /***************************************************/
    }
}

