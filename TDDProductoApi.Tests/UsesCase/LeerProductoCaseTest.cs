using Application.DTOs.Producto;
using Application.UseCases.ProductoCases;
using AutoMapper;
using Domain.Entitites;
using Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDProductoApi.Tests.UsesCase
{
    public class LeerProductoCaseTest
    {
        private readonly Mock<IProductoService> _productoServiceMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly LeerProductoCase _leerProductoCase;
        public LeerProductoCaseTest()
        {
            _productoServiceMock=new Mock<IProductoService>();
            _mapperMock = new Mock<IMapper>();

            _leerProductoCase = new LeerProductoCase(
                _productoServiceMock.Object,
                _mapperMock.Object
                );
        }

        [Fact]
        public async Task DebeLeerProducto_Y_RetornarDatos()
        {
            //A
            int id = 1;
            Producto producto = new Producto { Id = id, Nombre = "Detergente", Imagen = "imagen" };
            ProductoVerDTO dto = new ProductoVerDTO { id = id, name = "Detergente", image = "imagen" };

            _mapperMock.Setup(m=>m.Map<ProductoVerDTO>(producto))
                .Returns(dto);
            _productoServiceMock.Setup(p => p.BuscarProductoId(id))
                .ReturnsAsync(producto);

            //A
            var respuesta =await _leerProductoCase.LeerProductoId(id);

            //A
            Assert.Equal(id,respuesta.id);
            Assert.NotNull(respuesta);

        }

        [Fact]
        public async Task DebeLeerProducto_Y_No_RetornarDatos()
        {
            //A
            int id = 10;

            _productoServiceMock.Setup(p => p.BuscarProductoId(id))
                .ReturnsAsync((Producto?)null);

            //A
            var respuesta = await _leerProductoCase.LeerProductoId(id);

            //A
            Assert.Null(respuesta);

        }
    }
}
