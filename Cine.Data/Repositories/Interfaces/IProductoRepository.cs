using Cine.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Repositories.Interfaces
{
    public interface IProductoRepository
    {
         Task<List<ProductosShop>> ObtenerProductosShopAsync();
    }
}
