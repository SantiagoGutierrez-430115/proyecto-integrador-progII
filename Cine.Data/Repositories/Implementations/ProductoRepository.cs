using Cine.Data.Models;
using Cine.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Repositories.Implementations
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly CineDbContext _context;

        public ProductoRepository(CineDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductosShop>> ObtenerProductosShopAsync()
        {
            return await _context.ProductosShops.ToListAsync();
        }


    }
}
