using System;
using System.Collections.Generic;
using System.Text;

namespace IISManagerCore.Models
{
    public class WebSite
    {
        public Int32 Identity
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public String PhysicalPath
        {
            get;
            set;
        }

        public ServerState Status
        {
            get;
            set;
        }
    }
}
