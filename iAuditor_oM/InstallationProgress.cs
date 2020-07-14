using System.ComponentModel;
using System.Collections.Generic;
using BH.oM.Base;


namespace BH.oM.iAuditor
{

    [Description("Installation progress to compile your audit, please include image filepaths ")]
    public class InstallationProgress : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("General status update to be documented within the audit")]
        public virtual string Status { get; set; }

        [Description("Please provided a description tag for Elevation")]
        public virtual List<string> ElevationArea { get; set; }

        [Description("Filepath for image(s) associated with the comment")]
        public virtual List<string> ImageFilePaths { get; set; }

        /***************************************************/
    }
}

