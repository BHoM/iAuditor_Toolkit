using System.ComponentModel;
using System.Collections.Generic;
using BH.oM.Base;


namespace BH.oM.iAuditor
{

    [Description("Installation progress for a single audit, please include image filepaths as applicable")]
    public class InstallationProgress : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("General status of progress")]
        public virtual string Status { get; set; }

        [Description("Description for the area included")]
        public virtual string Area { get; set; }

        [Description("Media associated with the progress")]
        public virtual List<string> Media { get; set; }

        /***************************************************/
    }
}

