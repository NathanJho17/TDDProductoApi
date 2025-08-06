using Domain.Interfaces;

namespace Application;

public class BorrarProductoCase
{
    private readonly IProductoService productoService;

    public BorrarProductoCase(IProductoService productoService)
    {
        this.productoService = productoService;
    }

    public async Task<bool> Borrar(int id)
    {
        var productoEncontrado = await productoService.BuscarProductoId(id);
        if (productoEncontrado == null)
        {
            return false;
        }
        var borrar = productoService.BorrarProductoId(productoEncontrado);

        return borrar;
    }
}
