using Common.Entities;
using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase  // ✅ Cambia IUserController por ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()  // ✅ Usa ActionResult
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);  // ✅ Devuelve HTTP 200 con los datos
        }
    }
}