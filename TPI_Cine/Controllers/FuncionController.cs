using Cine.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPI_Cine.Controllers
{
    [ApiController]
    [Route("api/funciones")]
    public class FuncionController : ControllerBase
    {
        private readonly IFuncionService _funcionService;

        public FuncionController(IFuncionService funcionService)
        {
            _funcionService = funcionService;
        }

        [HttpGet("{idFuncion}/butacas")]
        public async Task<IActionResult> ObtenerButacas([FromRoute] int idFuncion)
        {
            if (idFuncion <= 0)
            {
                return BadRequest("El ID de la función debe ser mayor a cero.");
            }

            var butacas = await _funcionService.ObtenerButacasPorFuncionAsync(idFuncion);

            if (butacas is null)
            {
                return NotFound("La función indicada no existe.");
            }

            return Ok(butacas);
        }
    }
}
