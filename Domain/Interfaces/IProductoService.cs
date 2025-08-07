using Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductoService
    {
        Task<Producto> BuscarProductoId(int Id);

        Task<Producto> CrearProducto(Producto producto);

        bool BorrarProductoId(Producto producto);

        Task<Producto> ActualizarProducto(Producto producto);
    }
}
