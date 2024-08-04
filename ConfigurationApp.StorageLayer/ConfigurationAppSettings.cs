using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationApp.StorageLayer
{
    public class ConfigurationAppSettings
    {
        public string DefaultConnection { get; set; }
        public string ApplicationName { get; set; }
        public int RefreshTimerIntervalInMs { get; set; }
    }
}
