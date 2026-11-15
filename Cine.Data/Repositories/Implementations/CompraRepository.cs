using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cine.Data.DTOs;
using Cine.Data.Models;
using Cine.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cine.Data.Repositories.Implementations
{
    public class CompraRepository : ICompraRepository
    {
        private readonly CineDbContext _context;

        public CompraRepository(CineDbContext context)
        {
            _context = context;
        }

        public async Task<int?> ObtenerCodClientePorUsuarioAsync(int idUsuario)
        {
            return await _context.Clientes
                .Where(cliente => cliente.IdUsuario == idUsuario)
                .Select(cliente => (int?)cliente.CodCliente)
                .FirstOrDefaultAsync();
        }

        public async Task<Funcione?> ObtenerFuncionAsync(int idFuncion)
        {
            return await _context.Funciones
                .Include(funcion => funcion.IdSalaNavigation)
                .Include(funcion => funcion.IdHorarioNavigation)
                .FirstOrDefaultAsync(funcion => funcion.IdFuncion == idFuncion);
        }

        public async Task<(int NroFactura, decimal Total)> RegistrarCompraAsync(
            int codCliente,
            int idSucursal,
            int idCajero,
            int idFuncion,
            List<ButacaFuncionDto> butacasSeleccionadas)
        {
            if (butacasSeleccionadas.Count == 0)
            {
                throw new ArgumentException("La compra debe tener al menos una entrada.", nameof(butacasSeleccionadas));
            }

            var idCanalWeb = await _context.CanalesVentas
                .Where(canal => canal.CanalVenta == "Web")
                .Select(canal => (int?)canal.IdCanalVenta)
                .FirstOrDefaultAsync();

            var idConsumidorFinal = await _context.CondicionesIvas
                .Where(condicion => condicion.Condicion == "Consumidor Final")
                .Select(condicion => (int?)condicion.IdCondicionIva)
                .FirstOrDefaultAsync();

            var idTransferencia = await _context.FormasPagos
                .Where(formaPago => formaPago.FormaPago == "Transferencia")
                .Select(formaPago => (int?)formaPago.IdFormaPago)
                .FirstOrDefaultAsync();

            var idPendiente = await _context.EstadosCompras
                .Where(estado => estado.Estado == "Pendiente")
                .Select(estado => (int?)estado.IdEstadoCompra)
                .FirstOrDefaultAsync();

            var idPagada = await _context.EstadosCompras
                .Where(estado => estado.Estado == "Pagada")
                .Select(estado => (int?)estado.IdEstadoCompra)
                .FirstOrDefaultAsync();

            if (idCanalWeb is null || idConsumidorFinal is null || idTransferencia is null
                || idPendiente is null || idPagada is null)
            {
                throw new InvalidOperationException("Faltan datos de configuración para registrar la venta web.");
            }

            var factura = new Factura
            {
                CodCliente = codCliente,
                IdSucursal = idSucursal,
                IdCajero = idCajero,
                IdCanalVenta = idCanalWeb.Value,
                IdCondicionIva = idConsumidorFinal.Value,
                IdEstadoCompra = idPendiente.Value
            };

            decimal total = 0;
            foreach (var butaca in butacasSeleccionadas)
            {
                var entrada = new Entrada
                {
                    IdFuncion = idFuncion,
                    IdButaca = butaca.IdButaca,
                    Precio = butaca.Precio
                };

                factura.DetalleFacturas.Add(new DetalleFactura
                {
                    IdEntradaNavigation = entrada,
                    Cantidad = 1,
                    PrecioUnitario = entrada.Precio,
                    DescuentoUnitario = 0,
                    PrecioFinalUnitario = entrada.Precio
                });

                total += entrada.Precio;
            }

            factura.FacturasFormasPagos.Add(new FacturasFormasPago
            {
                IdFormaPago = idTransferencia.Value,
                MontoPagado = total
            });

            // La base exige que la factura esté pendiente al agregar detalles y pagos.
            using var transaccion = await _context.Database.BeginTransactionAsync();
            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            factura.IdEstadoCompra = idPagada.Value;
            await _context.SaveChangesAsync();

            await transaccion.CommitAsync();
            return (factura.NroFactura, total);
        }

        public async Task<int?> ObtenerIdCajeroPorSucursalAsync(int idSucursal)
        {
            return await _context.Cajeros
                .Where(cajero => cajero.IdSucursal == idSucursal)
                .OrderBy(cajero => cajero.IdCajero)
                .Select(cajero => (int?)cajero.IdCajero)
                .FirstOrDefaultAsync();
        }
    }
}
