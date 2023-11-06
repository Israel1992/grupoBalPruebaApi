using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using grupoBalPruebaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using grupoBalPruebaAPI.Services;
using Azure;

namespace grupoBalPruebaAPI.Controllers{

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AreasController : ControllerBase{
        private  AreaService areaService;

        public AreasController(GbPruebaContext context){
            areaService = new AreaService(context);
        }

        // GET: api/Areas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Area>>> GetAreas(){
            var response = await this.areaService.GetAreas<Area>();
            return Ok(response);
        }

        // GET: api/Areas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Area>> GetArea(int id){
            var response = await this.areaService.GetAreas<Area>(id);
            return Ok(response);
        }

        // PUT: api/Areas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArea(int id, Area area){ //Al model Area se agregan notaciones para validar campos
            var response = await this.areaService.PutArea<Area>(id, area);
            return Ok(response);
        }

        // POST: api/Areas
        [HttpPost]
        public async Task<ActionResult<Area>> PostArea(Area area){ //Al model Area se agregan notaciones para validar campos
            var response = await this.areaService.PostArea<Area>(area);
            return Ok(response);
        }

        // DELETE: api/Areas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArea(int id){
            var response = await this.areaService.DeleteArea<Area>(id);
            return Ok(response);
        }
    }
}
