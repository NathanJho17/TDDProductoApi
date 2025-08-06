using Application.UseCases.ProductoCases;
using Domain.Entitites;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDProductoApi.Tests.Services
{
    public class BuscarProductoIdTest
    {
        [Fact]
        public async Task ObtenerPorId_DeberiaRetornarProducto()
        {
            //Arrange

            // Crear opciones para usar la misma DB
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            var services = new ServiceCollection();

            // Registrar el contexto con las opciones ya creadas
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("TestDb"));

            // Registrar repositorio y servicio correctos
            services.AddScoped<IProductoService, ProductoRepository>();

            var provider = services.BuildServiceProvider();

            using (var scope = provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Productos.Add(new Producto { Id = 1, Nombre = "Teclado", Imagen = "tec.png" });
                context.SaveChanges();
            }

            // Act
            using (var scope = provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IProductoService>();
                var result = await service.BuscarProductoId(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Teclado", result.Nombre);
            }
        }

    }
}
