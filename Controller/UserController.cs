using Common.Entities;
using Common.helpers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Operaciones para gestionar usuarios - Consultas y listados")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [SwaggerOperation(
            Summary = "Obtener todos los usuarios",
            Description = "Retorna una lista completa de todos los usuarios existentes en el sistema.")]
        [SwaggerResponse(200, "Operación exitosa", typeof(List<User>))]
        [SwaggerResponse(500, "Error interno del servidor", typeof(ErrorResponse))]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Retorna un ErrorResponse estándar
                return StatusCode(500, new ErrorResponse($"Error interno del servidor: {ex.Message}", 500));
            }
        }

        [HttpGet("{userId}")]
        [SwaggerOperation(
            Summary = "Obtener usuario especifico",
            Description = "Retorna un usuario del sistema a travez se su id si este existe.")]
        [SwaggerResponse(200, "Operación exitosa", typeof(User))]
        [SwaggerResponse(400, "ID de Usuario invalido", typeof(ErrorResponse))]
        [SwaggerResponse(404, "Usuario no encontrado", typeof(ErrorResponse))]
        [SwaggerResponse(500, "Error interno del servidor", typeof(ErrorResponse))]
        public async Task<ActionResult<User>> GetUserById(
            [SwaggerParameter("ID del usuario", Required = true)] int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Retorna un ErrorResponse estándar
                return StatusCode(500, new ErrorResponse($"Error interno del servidor: {ex.Message}", 500));
            }
        }
    }
}
