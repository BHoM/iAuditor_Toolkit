using System.ComponentModel;
using System.Collections.Generic;
using BH.oM.Base;


namespace BH.oM.iAuditor
{

    [Description("An issue belonging to an audit")]
    public class Issue : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Issue number supplied to audit")]
        public virtual int IssueNumber { get; set; }

        [Description("Priority tag to better categorize your issue")]
        public virtual string Priority { get; set; } // this could be an enum of options
        
        [Description("Status description at time of observation")]
        public virtual string Status { get; set; }

        [Description("Provide a list of assignees for the issue")]
        public virtual string Assign { get; set; }

        [Description("Provide a description for the scope of the issue")]
        public virtual string Description { get; set; }

        [Description("Filepath for image(s) associated with the issue")]
        public virtual List<string> ImageFilePaths { get; set; }

        [Description("List of people issue is distributed to")]
        public virtual List<string> Distribution { get; set; }

        [Description("List of comments made on the issue")]
        public virtual List<Comment> Comments { get; set; }

        /***************************************************/
    }
}

