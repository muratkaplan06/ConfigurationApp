using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationApp.StorageLayer
{
    public interface IStorageProvider
    {
        Dictionary<string, object> LoadConfigurations(string applicationName);
        Task SaveConfigurationAsync(string applicationName, string name, object value, string type);
    }
}
