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


            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<ProductoVerDTO>(responseContent);
            Assert.Equal("Celular", responseJson?.name);

        }

        [Fact]
        public async Task BorrarProducto_DeberiaRetornarDosCientos()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory<Program>();
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var nuevoProducto = new Producto { Nombre = "Monitor_Led", Imagen = "monitor.jpg" };
            context.Productos.Add(nuevoProducto);
            context.SaveChanges();

            var productoId = nuevoProducto.Id;

            // Act
            var cliente = factory.CreateClient();
            var response = await cliente.DeleteAsync($"api/producto/{productoId}");
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<bool>(responseContent);
            Assert.True(responseJson);
        }

        [Fact]
        public async Task BorrarProducto_DeberiaRetornarCuatroCientos_ProductoNoEncontrado()
        {
            // Arrange
            int id = 9;
            var factory = new CustomWebApplicationFactory<Program>();
            using var scope = factory.Services.CreateScope();

            // Act
            var cliente = factory.CreateClient();
            var response = await cliente.DeleteAsync($"api/producto/{id}");

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Equal(responseContent, $"No existe el producto {id} para borrar");
        }

        [Fact]
        public async Task BorrarProducto_DeberiaRetornarQuinientos_Excepcion()
        {
            // Arrange
            var factoryError = new CustomWebApplicationFactoryWithError();
            var cliente = factoryError.CreateClient();

            // Act
            var response = await cliente.DeleteAsync($"api/producto/99");

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task EditarProducto_DeberiaRetornarDosCientos()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory<Program>();
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var nuevoProducto = new Producto { Nombre = "Monitor_Led", Imagen = "monitor.jpg" };
            context.Productos.Add(nuevoProducto);
            context.SaveChanges();
            var productoId = nuevoProducto.Id;

            // Act
            var cliente = factory.CreateClient();
            var dto = new ProductoEditarDTO { name = "Monitor_Led Editado", image = "monitor_editado.jpg" };
            var response = await cliente.PutAsync($"/api/producto/{productoId}", new StringContent(
                System.Text.Json.JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<ProductoVerDTO>(responseContent);
            Assert.Equal("Monitor_Led Editado", responseJson?.name);
        }

        [Fact]
        public async Task EditarProducto_DeberiaRetornarCuatroCientos_ProductoNoEncontrado()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory<Program>();
            var cliente = factory.CreateClient();
            var productoId = 99; // Producto que no existe
            var dto = new ProductoEditarDTO { name = "Producto Inexistente", image = "imagen_inexistente.jpg" };

            // Act
            var response = await cliente.PutAsync($"/api/producto/{productoId}", new
            StringContent(System.Text.Json.JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"
            ));

            //Assert 
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Equal($"Producto con ID {productoId} no encontrado.", responseContent);
        }

        [Fact]
        public async Task EditarProducto_DeberiaRetornarQuinientos_Excepcion()
        {
            // Arrange
            var factoryError = new CustomWebApplicationFactoryWithError();
           
            var cliente = factoryError.CreateClient();
            var dto = new ProductoEditarDTO { name = "Producto Inexistente", image = "imagen_inexistente.jpg" };
         
            // Act
            var response = await cliente.PutAsync($"/api/producto/99", new
            StringContent(System.Text.Json.JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json"
            ));
            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Equal($"No se puedo actualizar el producto", responseContent);
        }
    }
}
