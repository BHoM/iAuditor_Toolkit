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
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.ComponentModel;
using System.Web;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Adapters.iAuditor
{
    public static partial class Compute
    {
        [Description("Returns a bearer token for the CarbonQueryDatabase system from the provided username and password")]
        [Input("username", "Your username for the system")]
        [Input("password", "Your password for the system - case sensitive, do not share scripts with this saved")]
        [Output("bearerToken", "The bearer token to use the database system or the full response string if there was an error")]
        public static string BearerToken(string username, string password)
        {
            string apiAddress = "https://api.safetyculture.io/auth";
                
                System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Ssl3 |
                SecurityProtocolType.Tls12 |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls;

            HttpClient client = new HttpClient();

            var values = new Dictionary<string, string>
            {
                {"username", username},
                {"password", password},
                {"grant_type", "password"}
            };

            var content = new FormUrlEncodedContent(values);
            content.Headers.Clear();
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            HttpResponseMessage response = client.PostAsync(apiAddress, content).Result;
            if (response.IsSuccessStatusCode)
            {

                string responseAuthString = response.Content.ReadAsStringAsync().Result;
                if (responseAuthString.Split('"').Length >= 3)
                    return responseAuthString.Split('"')[3];
                else
                {
                    BH.Engine.Reflection.Compute.RecordError("We did not receive the response we expected. The response was '" + responseAuthString + "'");
                    return null;
                }
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("Request failed with code '" + response.StatusCode.ToString() + "'. Please check credentials and try again.");
                return null;
            }
        }
    }
}


