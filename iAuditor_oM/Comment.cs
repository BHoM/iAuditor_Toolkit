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

    [Description("Comment to be supplied to audit within iAuditor")]
    public class Comment : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Comment number supplied to audit")]
        public virtual int CommentNumber { get; set; }

        [Description("Priority tag to better categorize your comment")]
        public virtual string Priority { get; set; } // this could be an enum of options
        
        [Description("Status description at time of observation")]
        public virtual string Status { get; set; }

        [Description("Provide a list of assignees for the comment")]
        public virtual List<string> Assign { get; set; }

        [Description("Provide a description for the scope of the comment")]
        public virtual string Description { get; set; }

        [Description("Filepath for image(s) associated with the comment")]
        public virtual List<string> ImageFilePaths { get; set; }

        [Description("List of people comment is distributed to")]
        public virtual List<string> Distribution { get; set; }

        /***************************************************/
    }
}

