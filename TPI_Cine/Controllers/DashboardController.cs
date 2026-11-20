using Cine.Data.Exceptions;
using Cine.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TPI_Cine.Controllers
{
    // Indicadores del dashboard. Solo para el rol Admin (muestran recaudación).
    // Las fechas son opcionales y se mandan como yyyy-MM-dd (ej: ?desde=2026-11-10&hasta=2026-11-16).
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET api/dashboard/peliculas-mas-vendidas?desde=2026-11-10&hasta=2026-11-16&top=5
        [HttpGet("peliculas-mas-vendidas")]
        public async Task<IActionResult> PeliculasMasVendidas(
            [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta, [FromQuery] int top = 10)
        {
            try
            {
                var lista = await _dashboardService.GetPeliculasMasVendidasAsync(desde, hasta, top);
                return Ok(lista);
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener las películas más vendidas: {ex.Message}");
            }
        }

        // GET api/dashboard/horarios-mas-populares?desde=...&hasta=...
        [HttpGet("horarios-mas-populares")]
        public async Task<IActionResult> HorariosMasPopulares([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
        {
            try
            {
                var lista = await _dashboardService.GetHorariosMasPopularesAsync(desde, hasta);
                return Ok(lista);
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener los horarios más populares: {ex.Message}");
            }
        }

        // GET api/dashboard/formas-pago-mas-usadas?desde=...&hasta=...
        [HttpGet("formas-pago-mas-usadas")]
        public async Task<IActionResult> FormasPagoMasUsadas([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
        {
            try
            {
                var lista = await _dashboardService.GetFormasPagoMasUsadasAsync(desde, hasta);
                return Ok(lista);
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener las formas de pago más usadas: {ex.Message}");
            }
        }
    }
}
