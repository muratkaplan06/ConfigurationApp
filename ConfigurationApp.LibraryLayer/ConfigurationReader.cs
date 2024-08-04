using ConfigurationApp.StorageLayer;

namespace ConfigurationApp.LibraryLayer
{
    public class ConfigurationReader
    {
        private readonly string _applicationName;
        private readonly string _connectionString;
        private readonly int _refreshTimerIntervalInMs;
        private readonly Timer _timer;
        private readonly IStorageProvider _storageProvider; // Add this line
        private Dictionary<string, object> _configurations = new Dictionary<string, object>();

        // Modify the constructor to accept an IStorageProvider instance
        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs, IStorageProvider storageProvider)
        {
            _applicationName = applicationName;
            _connectionString = connectionString;
            _refreshTimerIntervalInMs = refreshTimerIntervalInMs;
            _storageProvider = storageProvider; // Assign the storageProvider

            LoadConfigurations();

            // Initialize and start the timer
            _timer = new Timer(RefreshConfigurations, null, _refreshTimerIntervalInMs, _refreshTimerIntervalInMs);
        }

        public T GetValue<T>(string key)
        {
            if (_configurations.TryGetValue(key, out var value))
            {
                return (T)value;
            }

            throw new KeyNotFoundException($"Key {key} not found.");
        }

        private void LoadConfigurations()
        {
            // Use the _storageProvider to load configurations
            _configurations = _storageProvider.LoadConfigurations(_applicationName);
        }

        private void RefreshConfigurations(object state)
        {
            // Use the _storageProvider to get updated configurations
            var updatedConfigurations = _storageProvider.LoadConfigurations(_applicationName);
            _configurations = updatedConfigurations;
        }
    }

}
