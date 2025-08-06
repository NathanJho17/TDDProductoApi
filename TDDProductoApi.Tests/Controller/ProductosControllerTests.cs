using Application.DTOs.Producto;
using Domain.Entitites;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;


namespace TDDProductoApi.Tests.Controller
{
    //Interfaz cuando la clase no tenga parametros en el constructor
   // public class ProductosControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>

    public class ProductosControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
          

        [Fact]
        public async Task GetProductoPorId_DeberiaRetornarProducto()
        {
            //Arrange
            var factory = new CustomWebApplicationFactory<Program>();
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var nuevoProducto = new Producto { Nombre = "Monitor_Led", Imagen = "monitor.jpg" };
            context.Productos.Add(nuevoProducto);
            context.SaveChanges();

            var productoId = nuevoProducto.Id;
            // Act
            var client = factory.CreateClient();
            var response = await client.GetAsync($"/api/producto/{productoId}");

            // Assert
            response.EnsureSuccessStatusCode(); // status 200
            var content = await response.Content.ReadAsStringAsync();
            var producto = System.Text.Json.JsonSerializer.Deserialize<ProductoVerDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(producto);
            Assert.Equal("Monitor_Led", producto.name);
        }

        [Fact]
        public async Task GetProductoPorId_DeberiaNotFound()
        {
            // Act
            var factory = new CustomWebApplicationFactory<Program>();
            var client = factory.CreateClient();
            var response = await client.GetAsync("/api/producto/10");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task CrearProducto_DeberiaRetoriarStatusDosCientosUno()
        {
            //Arrange
            var factory = new CustomWebApplicationFactory<Program>();
            var cliente = factory.CreateClient();
            var dto = new ProductoCrearDTO { image = "testt", name = "Celular" };
            var dtoJson = System.Text.Json.JsonSerializer.Serialize(dto);
            var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");
            //Act
            var response = await cliente.PostAsync("/api/producto", content);

            //Asserts
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);


            var responseContent =await response.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<ProductoVerDTO>(responseContent);
            Assert.Equal("Celular", responseJson?.name);
        }
    }
}
