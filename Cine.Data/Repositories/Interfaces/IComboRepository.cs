using Cine.Data.DTOs;
using Cine.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Repositories.Interfaces
{
    public interface IComboRepository
    {
        Task<ResultadoPaginadoDto<Combo>> GetAllAsync(string? descripcion, int? idTipoCombo, int pagina, int tamanio);
        Task<Combo?> GetByIdAsync(int id);
        Task<Dictionary<int, decimal>> GetPreciosProductosAsync(IEnumerable<int> idsProductos);
        Task<Combo> CreateAsync(Combo combo);
        Task<bool> UpdateAsync(int id, Combo datos);
        Task<bool> DeleteAsync(int id);
    }
}
