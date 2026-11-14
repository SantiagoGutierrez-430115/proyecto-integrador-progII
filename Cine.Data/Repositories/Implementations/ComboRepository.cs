using Cine.Data.DTOs;
using Cine.Data.Exceptions;
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
    public class ComboRepository : IComboRepository
    {
        private readonly CineDbContext _context;

        public ComboRepository(CineDbContext context)
        {
            _context = context;
        }

        private IQueryable<Combo> ConDetalle() =>
            _context.Combos
                .Include(c => c.IdTipoComboNavigation)
                .Include(c => c.ProductosCombos).ThenInclude(pc => pc.IdProductoShopNavigation)
                .AsSplitQuery();

        public async Task<ResultadoPaginadoDto<Combo>> GetAllAsync(
            string? descripcion, int? idTipoCombo, int pagina, int tamanio)
        {
            var query = ConDetalle();

            // Filtros opcionales
            if (!string.IsNullOrWhiteSpace(descripcion))
                query = query.Where(c => c.Descripcion.Contains(descripcion));
            if (idTipoCombo.HasValue)
                query = query.Where(c => c.IdTipoCombo == idTipoCombo.Value);

            // Total de combos que cumplen los filtros (antes de paginar)
            var total = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.IdCombo)
                .Skip((pagina - 1) * tamanio)
                .Take(tamanio)
                .ToListAsync();

            return new ResultadoPaginadoDto<Combo>
            {
                Items = items,
                Total = total,
                Pagina = pagina,
                Tamanio = tamanio
            };
        }

        public Task<Combo?> GetByIdAsync(int id) =>
            ConDetalle().FirstOrDefaultAsync(c => c.IdCombo == id);

        public async Task<Dictionary<int, decimal>> GetPreciosProductosAsync(IEnumerable<int> idsProductos)
        {
            var ids = idsProductos.Distinct().ToList();

            return await _context.ProductosShops
                .Where(p => ids.Contains(p.IdProductoShop))
                .ToDictionaryAsync(p => p.IdProductoShop, p => p.Precio);
        }

        public async Task<Combo> CreateAsync(Combo combo)
        {
            // Un solo SaveChanges: EF guarda el combo y sus productos en una transacción implícita
            _context.Combos.Add(combo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // El tipo de combo o alguno de los productos no existen
                throw new DatosInvalidosException(MensajeErrorGuardado);
            }

            return combo;
        }

        public async Task<bool> UpdateAsync(int id, Combo datos)
        {
            var combo = await _context.Combos
                .Include(c => c.ProductosCombos)
                .FirstOrDefaultAsync(c => c.IdCombo == id);

            if (combo == null) return false;

            combo.Descripcion = datos.Descripcion;
            combo.Precio = datos.Precio;
            combo.IdTipoCombo = datos.IdTipoCombo;

            // Se sacan los productos que ya no están en la lista
            var idsNuevos = datos.ProductosCombos.Select(x => x.IdProductoShop).ToList();
            var aQuitar = combo.ProductosCombos.Where(x => !idsNuevos.Contains(x.IdProductoShop)).ToList();
            _context.ProductosCombos.RemoveRange(aQuitar);

            // Los que ya estaban se actualizan; los nuevos se agregan
            foreach (var nuevo in datos.ProductosCombos)
            {
                var actual = combo.ProductosCombos.FirstOrDefault(x => x.IdProductoShop == nuevo.IdProductoShop);
                if (actual == null)
                {
                    combo.ProductosCombos.Add(new ProductosCombo
                    {
                        IdProductoShop = nuevo.IdProductoShop,
                        PrecioUnitario = nuevo.PrecioUnitario,
                        Cantidad = nuevo.Cantidad
                    });
                }
                else
                {
                    actual.PrecioUnitario = nuevo.PrecioUnitario;
                    actual.Cantidad = nuevo.Cantidad;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DatosInvalidosException(MensajeErrorGuardado);
            }

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var combo = await _context.Combos
                .Include(c => c.ProductosCombos)
                .FirstOrDefaultAsync(c => c.IdCombo == id);

            if (combo == null) return false;

            _context.ProductosCombos.RemoveRange(combo.ProductosCombos);
            _context.Combos.Remove(combo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ConflictoException("No se puede eliminar el combo porque está asociado a facturas o promociones.");
            }

            return true;
        }

        private const string MensajeErrorGuardado =
            "No se pudo guardar el combo: verificá que el tipo de combo y los productos existan, " +
            "y que el combo no esté asociado a facturas pagadas o anuladas.";
    }
}
