using Domain.Entitites;
using Domain.Interfaces;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace TDDProductoApi.Tests;

public class CustomWebApplicationFactoryWithError : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Elimina el registro real del servicio
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IProductoService));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Crea el mock
            var mockService = new Mock<IProductoService>();

            mockService.Setup(s => s.BuscarProductoId(It.IsAny<int>()))
                .ReturnsAsync((int id) => new Producto { Id = id, Nombre = "Mock", Imagen = "x.jpg" });

            mockService.Setup(s => s.BorrarProductoId(It.IsAny<Producto>()))
                .Throws(new Exception("Error al borrar"));

            mockService.Setup(s => s.ActualizarProducto(It.IsAny<Producto>()))
           .Throws(new Exception("No se puedo actualizar el producto"));

            services.AddScoped(_ => mockService.Object);
        });
    }
}