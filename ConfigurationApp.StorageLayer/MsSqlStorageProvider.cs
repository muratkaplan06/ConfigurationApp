using Microsoft.Data.SqlClient;

namespace ConfigurationApp.StorageLayer
{
    public class MsSqlStorageProvider : IStorageProvider
    {
        private readonly string _connectionString;

        public MsSqlStorageProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public Dictionary<string, object> LoadConfigurations(string applicationName)
        {
            var configurations = new Dictionary<string, object>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Name, Value, Type FROM Configurations WHERE ApplicationName = @ApplicationName AND IsActive = 1", connection);
                command.Parameters.AddWithValue("@ApplicationName", applicationName);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader["Name"].ToString();
                        string value = reader["Value"].ToString();
                        string type = reader["Type"].ToString();

                        object parsedValue;
                        switch (type.ToLower())
                        {
                            case "string":
                                parsedValue = value;
                                break;
                            case "int":
                                parsedValue = int.Parse(value);
                                break;
                            case "bool":
                                parsedValue = ParseBoolean(value);
                                break;
                            default:
                                throw new InvalidOperationException($"Unsupported configuration type: {type}");
                        }

                        configurations[name] = parsedValue;
                    }
                }
            }

            return configurations;
        }

        public async Task SaveConfigurationAsync(string applicationName, string name, object value, string type)
        {
            // Ensure the value is converted to a string before saving
            string valueString = value?.ToString();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO Configurations (ApplicationName, Name, Value, Type, IsActive) VALUES (@ApplicationName, @Name, @Value, @Type, 1)", connection);
                command.Parameters.AddWithValue("@ApplicationName", applicationName);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Value", valueString);
                command.Parameters.AddWithValue("@Type", type);

                await command.ExecuteNonQueryAsync();
            }
        }



        private bool ParseBoolean(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("The value cannot be null or empty.", nameof(value));
            }

            value = value.Trim().ToLower();
            if (value == "true" || value == "1")
            {
                return true;
            }
            if (value == "false" || value == "0")
            {
                return false;
            }

            throw new FormatException($"The value '{value}' is not recognized as a valid boolean.");
        }
    }
}
