using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace IISManagerCore.Models
{
    public class WindowsService
    {
        public string ServiceName { get; set; }

        public ServiceControllerStatus Status { get; set; }

        public string DisplayName { get; set; }

        public bool CanStop { get; set; }

        public bool CanShutdown { get; set; }

        public bool CanPauseAndContinue { get; set; }

        public ServiceStartMode StartType { get; set; }        
    }
}
