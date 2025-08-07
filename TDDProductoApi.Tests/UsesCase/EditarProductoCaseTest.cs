using Application.DTOs.Producto;
using Application.UseCases.ProductoCases;
using AutoMapper;
using Domain.Entitites;
using Domain.Interfaces;
using Moq;

namespace TDDProductoApi.Tests.UsesCase;

public class EditarProductoCaseTest
{
    private readonly Mock<IProductoService> _productoServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly EditarProductoCase _editarProductoCase;
    public EditarProductoCaseTest()
    {
        _mapperMock = new Mock<IMapper>();
        _productoServiceMock = new Mock<IProductoService>();
        _editarProductoCase = new EditarProductoCase(_productoServiceMock.Object,
        _mapperMock.Object);
    }

    [Fact]
    public async Task EditarProducto_CuandoProductoExiste_EntoncesActualizaProducto()
    {
        // Arrange
        int productoId = 1;
        var productoExistente = new Producto { Id = productoId, Nombre = "Producto1", Imagen = "imagen_existente.png" };
        ProductoEditarDTO productoEditDto = new ProductoEditarDTO { name = "Producto1 Editado", image = "imagen_editar.png" };
        var productoEditar = new Producto { Id = productoId, Nombre = productoEditDto.name, Imagen = productoEditDto.image };
        ProductoVerDTO productoVerDto = new ProductoVerDTO { name = productoEditDto.name, image = productoEditDto.image, id = productoId };


        _productoServiceMock.Setup(repo => repo.BuscarProductoId(productoId))
            .ReturnsAsync(productoExistente);

        _mapperMock.Setup(m => m.Map<Producto>(productoEditDto))
        .Returns(productoEditar);

        _productoServiceMock.Setup(repo => repo.ActualizarProducto(productoEditar))
            .ReturnsAsync(productoEditar);

        _mapperMock.Setup(m => m.Map<ProductoVerDTO>(productoEditar))
        .Returns(productoVerDto);
        //Act
        var resultado = await _editarProductoCase.Editar(productoId, productoEditDto);
        // Assert

        Assert.NotNull(resultado);
        Assert.Equal(productoEditDto.name, resultado.name);
    }

    [Fact]
    public async Task EditarProducto_NoEncontrado_RetornaExcepcion()
    {
        // Arrange
        int productoId = 1;
        _productoServiceMock.Setup(repo => repo.BuscarProductoId(productoId)).ThrowsAsync(new Exception($"No se puedo actualizar el producto con ID {productoId}"));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => _editarProductoCase.Editar(productoId, new ProductoEditarDTO()));

        // Assert
        Assert.Equal($"No se puedo actualizar el producto con ID {productoId}", exception.Message);

    }

    [Fact]
    public async Task EditarProducto_ErrorAlActualizar_RetornaNull()
    {
        // Arrange
        int productoId = 1;
        ProductoEditarDTO productoEditDto = new ProductoEditarDTO { name = "Producto1 Editado", image = "imagen_editar.png" };

        _productoServiceMock.Setup(repo => repo.BuscarProductoId(productoId))
        .ReturnsAsync((Producto?)null);

        //Act

        var resultado = await _editarProductoCase.Editar(productoId, productoEditDto);

        // Assert
        Assert.Null(resultado);
    }
}
