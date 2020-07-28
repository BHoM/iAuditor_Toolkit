using System.ComponentModel;
using System.Collections.Generic;
using BH.oM.Base;


namespace BH.oM.iAuditor
{

    [Description("An comment on an issue")]
    public class Comment : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Comment message")]
        public virtual string Message{ get; set; }

        [Description("Name of individual making the comment")]
        public virtual string Owner { get; set; }

        [Description("Date of comment")]
        public virtual string CommentDate { get; set; }

        /***************************************************/
    }
}

