using System;
using Asp.Versioning;
using Cortside.Authorization.WebApi.Models.Responses;
using Cortside.Health.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Cortside.Authorization.WebApi.Controllers {
    /// <summary>
    /// Settings
    /// </summary>
    [ApiVersionNeutral]
    [Route("api/settings")]
    [ApiController]
    [Produces("application/json")]
    public class SettingsController : ControllerBase {
        /// <summary>
        /// Config
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// SettingsController constructor
        /// </summary>
        public SettingsController(IConfiguration configuration) {
            Configuration = configuration;
        }

        /// <summary>
        /// Service settings that a consumer may need to be aware of
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(SettingsModel), 200)]
        [ResponseCache(CacheProfileName = "Default")]
        public IActionResult Get() {
            var result = GetSettingsModel();
            return Ok(result);
        }

        private SettingsModel GetSettingsModel() {
            var build = Configuration.GetSection("Build");

            return new SettingsModel() {
                Build = new BuildModel() {
                    Version = build.GetValue<string>("version"),
                    Timestamp = build.GetValue<DateTime>("timestamp"),
                    Tag = build.GetValue<string>("tag"),
                    Suffix = build.GetValue<string>("suffix")
                },
                Configuration = new ConfigurationModel() {

                },
                Service = Configuration["Service:Name"]
            };
        }
    }
}
