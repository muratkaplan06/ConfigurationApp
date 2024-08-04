using ConfigurationApp.LibraryLayer;
using ConfigurationApp.StorageLayer;
using ConfigurationApp.WebUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationApp.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly ConfigurationReader _configurationReader;
        private readonly IStorageProvider _storageProvider; // Add this line

        public ConfigurationController(ConfigurationReader configurationReader,
            IStorageProvider storageProvider)
        {
            _configurationReader = configurationReader;
            _storageProvider = storageProvider; // Assign the storageProvider
        }

        [HttpGet("{key}")]
        public IActionResult GetConfigurationValue(string key)
        {
            try
            {
                var value = _configurationReader.GetValue<object>(key);
                return Ok(value);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("save")]
        public async Task<IActionResult> SaveConfiguration([FromBody] SaveConfigurationRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.ApplicationName) || string.IsNullOrEmpty(request.Name) || request.Value == null || string.IsNullOrEmpty(request.Type))
            {
                return BadRequest("Invalid configuration data.");
            }

            try
            {
                await _storageProvider.SaveConfigurationAsync(request.ApplicationName, request.Name, request.Value, request.Type);
                return Ok("Configuration saved successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

}
