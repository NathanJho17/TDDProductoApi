using Application;
using Application.DTOs.Producto;
using Application.UseCases.ProductoCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly LeerProductoCase _leerProductoCase;
        private readonly CrearProductoCase _crearProductoCase;
        private readonly BorrarProductoCase _borrarProductoCase;

        public ProductoController(LeerProductoCase leerProductoCase, CrearProductoCase crearProductoCase,
        BorrarProductoCase borrarProductoCase)
        {
            _leerProductoCase = leerProductoCase;
            _crearProductoCase = crearProductoCase;
            _borrarProductoCase = borrarProductoCase;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoVerDTO>> getProductId(int id)
        {
            var res = await _leerProductoCase.LeerProductoId(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ProductoVerDTO>> createProduct([FromBody] ProductoCrearDTO dto)
        {
            var res = await _crearProductoCase.Crear(dto);
            if (res == null)
            {
                return BadRequest("Error en la solicitud");
            }
            return CreatedAtAction(nameof(getProductId), new { id = res.id }, res);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> deleteProduct(int id)
        {
            try
            {
                var res = await _borrarProductoCase.Borrar(id);
                if (res==false)
                {
                    return BadRequest($"No existe el producto {id} para borrar");
                }

                return Ok(res);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
