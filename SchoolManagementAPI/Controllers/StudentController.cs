using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpPost("hello")]
        public async Task<IActionResult> GetStudentDTO()
        {
            return Ok("hello");
        }
    }
}
