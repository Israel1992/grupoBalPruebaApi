
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using grupoBalPruebaAPI.Services;
using grupoBalPruebaAPI.Models.Request;
using grupoBalPruebaAPI.Models;

namespace grupoBalPruebaAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        
        private EmpleadoService empleadoService;

        public EmpleadosController(GbPruebaContext context){
            empleadoService = new EmpleadoService(context);
        }

        // GET: api/Empleadoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
        {
            var response = await this.empleadoService.GetEmpleados<Empleado>();
            return Ok(response);
        }

        // GET: api/Empleadoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> GetEmpleado(int id){
            var response = await this.empleadoService.GetEmpleado<Empleado>(id);
            return Ok(response);
        }

        // PUT: api/Empleadoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpleado(int id, EmpleadoRequest empleado)
        {
            var response = await this.empleadoService.PutEmpleado<EmpleadoRequest>(id, empleado);
            return Ok(response);
        }

        // POST: api/Empleadoes
        [HttpPost]
        public async Task<ActionResult<EmpleadoRequest>> PostEmpleado(EmpleadoRequest empleado)
        {
            var response = await this.empleadoService.PostEmpleado<EmpleadoRequest>(empleado);
            return Ok(response);
        }

        // DELETE: api/Empleadoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            var response = await this.empleadoService.DeleteEmpleado<Empleado>(id);
            return Ok(response);
        }

    }
}
