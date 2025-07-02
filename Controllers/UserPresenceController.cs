using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WHOTrackingWebAPI.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using WHOTrackingWebAPI.Helpers;

namespace WHOTrackingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPresenceController : ControllerBase
    {
        private readonly ILogger<UserPresenceController> _logger;
        private readonly IWebHostEnvironment _env;

        public UserPresenceController(ILogger<UserPresenceController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserPresenceDto presence)
        {
            try
            {
                if (presence == null || string.IsNullOrEmpty(presence.UserName))
                    return BadRequest("Invalid presence data");

                string folderPath = Path.Combine(_env.ContentRootPath, "UserPresenceLogs");
                Directory.CreateDirectory(folderPath);
                UserPresence userPresence = new UserPresence();

                userPresence.IpAddress = presence.IpAddress;
                userPresence.UserName = presence.UserName;
                string location = userPresence.IsOffice ? "Office" : "Home";
                string filePath = Path.Combine(folderPath, "AttendenceSheet.xlsx");
                ExcelHelper.WriteUserPresence(filePath, userPresence.IpAddress, location, userPresence.UserName);

                _logger.LogInformation($"Presence recorded for {presence.UserName} at {DateTime.Now} with IP {presence.IpAddress}");

                return Ok("Presence recorded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write user presence data");
                return StatusCode(500, "Internal server error.");
            }
        }
    }

}
