using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Cine.Data.DTOs;
using Cine.Data.Exceptions;
using Cine.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace TPI_Cine.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/compras")]
    public class CompraController : ControllerBase
    {
        private readonly ICompraService _compraService;

        public CompraController(ICompraService compraService)
        {
            _compraService = compraService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearCompra([FromBody] CrearCompraDto dto)
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(idUsuarioClaim, out int idUsuario))
            {
                return Unauthorized("No se pudo identificar al usuario autenticado.");
            }

            try
            {
                var compra = await _compraService.CrearCompraAsync(idUsuario, dto);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    nroFactura = compra.NroFactura,
                    total = compra.Total
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ConflictoException ex)
            {
                return Conflict(ex.Message);
            }
            catch (DbUpdateException ex) when (EsConflictoDeButaca(ex))
            {
                return Conflict("Una de las butacas seleccionadas ya no está disponible.");
            }
        }

        private static bool EsConflictoDeButaca(DbUpdateException ex)
        {
            if (ex.InnerException is not SqlException errorSql)
            {
                return false;
            }

            // El trigger rechaza una reserva activa y la restricción impide vender dos veces.
            bool reservaActiva = errorSql.Number == 50009;
            bool entradaDuplicada = (errorSql.Number == 2601 || errorSql.Number == 2627)
                && errorSql.Message.Contains("UQ_entradas_funcion_butaca");

            return reservaActiva || entradaDuplicada;
        }
    }
}
