using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace News_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class connectsController : ControllerBase
    {
        readonly IConfiguration _configuration;
        public connectsController( IConfiguration configuration)
        {
            _configuration = configuration;
        }
       [HttpGet("health")]
       [Authorize(Roles = "Admin")] // Require authentication
    public IActionResult HealthCheck()
    {
    try
    {
        using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("conn")))
        {
            conn.Open();
            return Ok(new { status = "healthy" }); // Generic response only
        }
    }
    catch
    {
        return StatusCode(503, new { status = "unhealthy" }); // No error details
    }
}
    }
}
