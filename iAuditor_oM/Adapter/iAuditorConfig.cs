/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
 
using BH.oM.Adapter;
using System.Collections.Generic;
using System.ComponentModel;

namespace BH.oM.Adapters.iAuditor
{
    [Description("This Config can be specified in the `ActionConfig` input of any Adapter Action (e.g. Push).")]
    // Note: this will get passed within any CRUD method (see their signature). 
    // In order to access its properties, you will need to cast it to `CarbonQueryDatabaseActionConfig`.
    public class iAuditorConfig : ActionConfig
    {
        /***************************************************/
        /**** Public Properties                         ****/
        /***************************************************/

        [Description("Specifies ID of the Audit to search for.")]
        public virtual string Id { get; set; } = null;

        [Description("Path to pull/push media assets to/from that relate to the audit.")]
        public virtual string AssetFilePath { get; set; } = @"C:\BHoM\iAuditor Assets";

        [Description("Set to true to download / upload asset file sassociated with the audit.")]
        public virtual bool IncludeAssetFiles { get; set; } = true;


        /***************************************************/
    }
}


