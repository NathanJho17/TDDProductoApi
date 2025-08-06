using System.Threading.Tasks;
using Application;
using Domain.Entitites;
using Domain.Interfaces;
using Moq;

namespace TDDProductoApi.Tests.UsesCase;

public class BorrarProductoCaseTest
{
    private Mock<IProductoService> _productServiceMock;

    private BorrarProductoCase _borrarProductoCase;
    public BorrarProductoCaseTest()
    {
        _productServiceMock = new Mock<IProductoService>();

        _borrarProductoCase = new BorrarProductoCase(
            _productServiceMock.Object
        );
    }

  

    [Fact]
    public async Task DebeBorrarProductoExitosamente()
    {
        //Arrange
        int id = 99;
        Producto mockProducto = new Producto() { Id = id, Imagen = "image.png", Nombre = "Para borrar" };
        _productServiceMock.Setup(p => p.BuscarProductoId(id))
        .ReturnsAsync(mockProducto);
        _productServiceMock.Setup(b => b.BorrarProductoId(mockProducto))
        .Returns(true);

        //Act
        var borra = await _borrarProductoCase.Borrar(id);

        //Asserts

        Assert.True(borra);


    }

    [Fact]
    public async Task DebeDevolverExepcionAlBorrarProducto()
    {
        // Arrange
        int id = 1;
        Producto mockProducto = new Producto() { Id = id, Imagen = "image.png", Nombre = "No debe borrar" };
        _productServiceMock.Setup(p => p.BuscarProductoId(id))
        .ReturnsAsync(mockProducto);

        _productServiceMock.Setup(m => m.BorrarProductoId(mockProducto))
        .Throws(new Exception("Error al borrar producto"));

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => _borrarProductoCase.Borrar(id));

        // Assert
        Assert.Equal("Error al borrar producto", exception.Message);
    }

    [Fact]
    public async Task NoDebeObtenerProductoConIdYNoBorrar()
    {
        // Arrange
        int id = 3;
        _productServiceMock.Setup(p => p.BuscarProductoId(id))
        .ReturnsAsync((Producto?)null);
        // Act
        var borrar = await _borrarProductoCase.Borrar(id);
        // Assert
        Assert.False(borrar);
    }
}
