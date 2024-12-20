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
using BH.oM.Base;
using BH.Adapter;
using System.Net;
using System.Net.Http;
using System.ComponentModel;
using BH.oM.Base.Attributes;
using BH.Engine.Adapters.iAuditor;

namespace BH.Adapter.iAuditor
{
    public partial class iAuditorAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        [Description("Adapter to connect to iAuditor.")]
        [Input("username", "Provide iAuditor Username.")]
        [Input("password", "Provide iAuditor Password.")]
        [Input("token", "Provide iAuditor token instead of username and password.")]
        [Output("adapter", "Adapter results.")]
        public iAuditorAdapter(string username = "", string password = "", string token = null, bool active = false)
        {
            if (active)
            {
                m_bearerToken = token ?? Compute.BearerToken(username, password);
            }
            else
            {
                m_bearerToken = null;
            }
        }

        /***************************************************/
        /*** Private Fields                              ***/
        /***************************************************/

        private static string m_bearerToken = null;

    }
}




