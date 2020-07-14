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
        public virtual string Assign { get; set; }

        [Description("Provide a description for the scope of the comment")]
        public virtual string Description { get; set; }

        [Description("Filepath for image(s) associated with the comment")]
        public virtual List<string> ImageFilePaths { get; set; }

        [Description("List of people comment is distributed to")]
        public virtual List<string> Distribution { get; set; }

        /***************************************************/
    }
}

