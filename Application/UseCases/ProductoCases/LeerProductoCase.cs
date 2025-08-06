using Application.DTOs.Producto;
using AutoMapper;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.ProductoCases
{
    public class LeerProductoCase
    {
        private readonly IProductoService _productoService;
        private readonly IMapper _mapper;

        public LeerProductoCase(IProductoService productoService,IMapper mapper)
        {
            _productoService = productoService;
            _mapper = mapper;
        }

        public async Task<ProductoVerDTO> LeerProductoId(int Id)
        {
            var producto= await _productoService.BuscarProductoId(Id);

            return _mapper.Map<ProductoVerDTO>(producto);
        }
    }
}
