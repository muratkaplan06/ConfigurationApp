namespace ConfigurationApp.WebUI.Models
{
    public class SaveConfigurationRequest
    {
        public string ApplicationName { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public string Type { get; set; }
    }
}
