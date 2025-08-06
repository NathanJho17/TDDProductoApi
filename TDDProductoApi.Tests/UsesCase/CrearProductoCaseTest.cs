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
    public class CrearProductoCaseTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IProductoService> _productoServiceMock;

        private readonly CrearProductoCase _crearProductoCase;

        public CrearProductoCaseTest()
        {
            _mapperMock = new Mock<IMapper>();
            _productoServiceMock = new Mock<IProductoService>();

            _crearProductoCase = new CrearProductoCase(
                _productoServiceMock.Object,
                _mapperMock.Object
            );


        }

        [Fact]
        public async Task DebeCrearProductoExitosamente()
        {
            //Arrange
            ProductoCrearDTO dtocrear = new ProductoCrearDTO { name = "Crear", image = "imm.jpg" };
            ProductoVerDTO dtoVer = new ProductoVerDTO {id=1, name = "Crear", image = "imm.jpg" };

            Producto producto = new Producto { Nombre = "Crear", Imagen = "imm.jpg" };

            _mapperMock.Setup(m=>m.Map<Producto>(dtocrear))
                .Returns(producto);

            _productoServiceMock.Setup(c=>c.CrearProducto(producto)).
                ReturnsAsync(producto);

            _mapperMock.Setup(m => m.Map<ProductoVerDTO>(producto))
              .Returns(dtoVer);

            //Act
            var respuesta =  await _crearProductoCase.Crear(dtocrear);

            //Asserts

            Assert.NotNull(respuesta);
            Assert.Equal("Crear", respuesta.name);

        }
    }
}
