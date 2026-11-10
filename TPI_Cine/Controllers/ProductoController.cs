using Microsoft.AspNetCore.Mvc;
using Cine.Data.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace TPI_Cine.Controllers
{
    [ApiController]
    [Route("api/funciones")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet("/productos")]
        public async Task<IActionResult> ObtenerProductos()
        {
            var productos = await _productoService.ObtenerProductosShopAsync();
            if(productos is null)
            {
                return NotFound("No hay productos que mostrar");
            }    
            return Ok(productos);
        }

    }
}
