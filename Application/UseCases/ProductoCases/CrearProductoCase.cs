using Application.DTOs.Producto;
using AutoMapper;
using Domain.Entitites;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.UseCases.ProductoCases
{
    public class CrearProductoCase
    {
        private readonly IProductoService _productoService;
        private readonly IMapper _mapper;

        public CrearProductoCase(IProductoService productoService, IMapper mapper)
        {
            _productoService = productoService;
            _mapper = mapper;
        }

        public async Task<ProductoVerDTO?> Crear(ProductoCrearDTO dto)
        {
            var map=_mapper.Map<Producto>(dto);

            var crear=await _productoService.CrearProducto(map);

            return (crear != null) ? _mapper.Map<ProductoVerDTO>(crear) : null;
           
        }
    }
}
