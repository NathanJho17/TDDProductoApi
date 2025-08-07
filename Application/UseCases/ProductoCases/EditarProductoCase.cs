using Application.DTOs.Producto;
using AutoMapper;
using Domain.Entitites;
using Domain.Interfaces;

namespace Application.UseCases.ProductoCases;

public class EditarProductoCase
{
    private readonly IProductoService _productoService;
    private readonly IMapper _mapper;

    public EditarProductoCase(IProductoService productoService, IMapper mapper)
    {
        _productoService = productoService;
        _mapper = mapper;
    }

    public async Task<ProductoVerDTO> Editar(int productoId, ProductoEditarDTO productoEditDto)
    {
        var productoExistente = await _productoService.BuscarProductoId(productoId);
        if (productoExistente == null)
        {
            return null;
        }
        var producto = _mapper.Map<Producto>(productoEditDto);
        var editar = await _productoService.ActualizarProducto(producto);

        var productoVerDto = _mapper.Map<ProductoVerDTO>(editar);
        return productoVerDto;
    }
}
