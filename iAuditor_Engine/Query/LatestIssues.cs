/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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
using System.Web;
using BH.oM.Base.Attributes;
using BH.oM.Inspection;

namespace BH.Engine.Adapters.iAuditor
{
    public static partial class Query
    {
        [Description("Returns the latest unique instance of each Issue from a chronological list of Issues.")]
        [Input("allIssues", "All Issues, potentially including duplicates of modified Issues. This list is assumed to be chronological from oldest to newest.")]
        [Output("latestIssues", "The latest instance of each unique Issue included in the provided collection of Issues.")]
        public static List<Issue> LatestIssues(this List<Issue> allIssues)
        {
            allIssues.Reverse();
            List<Issue> result = allIssues
                .OrderBy(x => x.IssueNumber)
                .GroupBy(x => x.IssueNumber)
                .Select(g => g.First())
                .ToList();
            return result;
        }
    }
}





