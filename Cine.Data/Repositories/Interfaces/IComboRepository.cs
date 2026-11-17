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

        // Precio actual de cada producto del shop que exista, por id. Los ids que no existen no aparecen
        Task<Dictionary<int, decimal>> GetPreciosProductosAsync(IEnumerable<int> idsProductos);

        // El combo llega con su lista ProductosCombos cargada. Lanza DatosInvalidosException si la base rechaza el insert
        Task<Combo> CreateAsync(Combo combo);

        // Reemplaza los datos y los productos del combo. Devuelve false si no existe.
        // Lanza DatosInvalidosException si la base rechaza el cambio
        Task<bool> UpdateAsync(int id, Combo datos);

        // Borra el combo y sus productos. Devuelve false si no existe.
        // Lanza ConflictoException si el combo está en facturas o promociones
        Task<bool> DeleteAsync(int id);
    }
}
