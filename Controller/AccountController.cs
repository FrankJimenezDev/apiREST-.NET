using Common.Entities;
using Common.helpers;
using Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Operaciones para gestionar cuentas bancarias - Consultas y balances")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(
            Summary = "Obtener todas las cuentas bancarias",
            Description = "Retorna una lista completa de todas las cuentas bancarias existentes en el sistema.")]
        [SwaggerResponse(200, "Operación exitosa", typeof(List<Account>))]
        [SwaggerResponse(401, "Token inválido o no proporcionado", typeof(ErrorResponse))]
        [SwaggerResponse(500, "Error interno del servidor", typeof(ErrorResponse))]
        public async Task<ActionResult<List<Account>>> GetAllAccounts()
        {
            try
            {
                var accounts = await _accountService.GetAllAccountsAsync();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("Error interno del servidor", 400));
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Obtener cuentas por usuario",
            Description = "Retorna todas las cuentas asociadas a un usuario específico mediante su ID.")]
        [SwaggerResponse(200, "Cuentas encontradas", typeof(List<Account>))]
        [SwaggerResponse(400, "ID de usuario inválido", typeof(ErrorResponse))]
        [SwaggerResponse(401, "Token inválido o no proporcionado", typeof(ErrorResponse))]
        [SwaggerResponse(404, "Usuario no encontrado o sin cuentas", typeof(ErrorResponse))]
        [SwaggerResponse(500, "Error interno del servidor", typeof(ErrorResponse))]
        public async Task<ActionResult<List<Account>>> GetAccountsByUserId(
            [SwaggerParameter("ID del usuario", Required = true)] int userId)
        {
            try
            {
                if (userId <= 0)
                    return BadRequest(new ErrorResponse("El ID de usuario debe ser mayor a 0", 400));

                var accounts = await _accountService.GetAccountsByUserIdAsync(userId);

                if (accounts == null || !accounts.Any())
                    return NotFound(new ErrorResponse($"No se encontraron cuentas para el usuario {userId}", 401));

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse($"Error al obtener cuentas del usuario: {ex.Message}", 500));
            }
        }

        [HttpGet("balance/{accountId}")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Obtener balance de cuenta",
            Description = "Retorna el balance actual de una cuenta bancaria específica mediante su ID.")]
        [SwaggerResponse(200, "Balance obtenido exitosamente", typeof(BalanceResponse))]
        [SwaggerResponse(400, "ID de cuenta inválido", typeof(ErrorResponse))]
        [SwaggerResponse(401, "Token inválido o no proporcionado", typeof(ErrorResponse))]
        [SwaggerResponse(404, "Cuenta no encontrada", typeof(ErrorResponse))]
        [SwaggerResponse(500, "Error interno del servidor", typeof(ErrorResponse))]
        public async Task<ActionResult<decimal>> GetBalanceByAccountId(
            [SwaggerParameter("ID de la cuenta", Required = true)] int accountId)
        {
            try
            {
                if (accountId <= 0)
                    return BadRequest(new ErrorResponse("El ID de cuenta debe ser mayor a 0", 400));

                var balance = await _accountService.GetBalanceOfAnAccountAsync(accountId);
                return Ok(new BalanceResponse { AccountId = accountId, Balance = balance });
            }
            catch (Exception ex) when (ex.Message.Contains("not found"))
            {
                return NotFound(new ErrorResponse($"Cuenta con ID {accountId} no encontrada", 401));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse($"Error al obtener balance: {ex.Message}", 500));
            }
        }
    }

    // Clases para respuestas estandarizadas

    public class BalanceResponse
    {
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}