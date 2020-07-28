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

        [Description("Path to pull/push media assets to/from that relate to the audit.")]
        public virtual string AssetFilePath { get; set; } = @"C:\BHoM\iAuditor Assets";

        [Description("Set to true to download / upload asset file sassociated with the audit.")]
        public virtual bool IncludeAssetFiles { get; set; } = true;


        /***************************************************/
    }
}
