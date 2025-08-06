using Domain.Entitites;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ProductoRepository : IProductoService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductoRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async  Task<Producto> BuscarProductoId(int Id)
        {
            return await _dbContext.Productos.FindAsync(Id);
        }

        public async Task<Producto> CrearProducto(Producto producto)
        {
            try
            {
                _dbContext.Add(producto);
                await _dbContext.SaveChangesAsync();

                return producto;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
