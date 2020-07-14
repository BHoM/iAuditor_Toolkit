using BH.oM.Adapter;
using System.Collections.Generic;
using System.ComponentModel;

namespace BH.oM.Adapter.iAuditor
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


        /***************************************************/
    }
}
