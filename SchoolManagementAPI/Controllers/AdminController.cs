using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.Repositories.Repo;
using SchoolManagementAPI.RequestResponse.Request;

namespace SchoolManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;

        public AdminController(IAdminRepository adminRepository, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
        }

        [HttpPost("/admin-create")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountCreateRequest request )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var admin = _mapper.Map<Admin>(request);
            await _adminRepository.Create(admin);
            return Ok(admin);
        }
        [HttpPost("/admin-login/")]
        public async Task<IActionResult> Create([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var admin = await _adminRepository.GetbyUsername(request.Username);
            if (admin == null || admin.Password != request.Password)
                return BadRequest("not found username");
            return Ok(admin);
        }

        [HttpGet("/admin-get-all")]
        public async Task<IActionResult> GetAll()
        {
            var admins = await _adminRepository.GetAll();
            return Ok(admins);
        }
    }
}
