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
 
using BH.oM.Base;
using System.Collections.Generic;
using BH.oM.Adapters.HTTP;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.Adapters.iAuditor
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public  Method                            ****/
        /***************************************************/

        [Description("Create a GetRequest for iAuditor")]
        [Input("apiCommand", "The iAuditor REST API command to create a GetRequest with")]
        [Input("bearerToken", "The iAuditor bearerToken (this can be acquired using Compute BearerToken with your EC3 username and password)")]
        [Input("parameters", "An optional CustomObject with properties representing parameters to create the GetRequest with (ie count, name_like, etc)")]
        [Output("GetRequest", "A GetRequest with CarbonQueryDatabase specific headers and uri")]

        public static GetRequest iAuditorRequest(string apiCommand, string bearerToken, CustomObject parameters = null)
        {
            return new BH.oM.Adapters.HTTP.GetRequest
            {
                BaseUrl = "https://api.safetyculture.io/" + apiCommand,
                Headers = new Dictionary<string, object>()
                {
                    { "Authorization", "Bearer " +  bearerToken }
                },
                Parameters = parameters?.CustomData
        };
        }

        /***************************************************/
    }
}

