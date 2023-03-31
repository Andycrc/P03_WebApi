using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P03_WebApi.Models;

namespace P03_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class marcasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public marcasController(equiposContext equiposContext)
        {
            _equiposContexto = equiposContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            List<marcas> listadoEquipo = (from e in _equiposContexto.marcas
                                           select e).ToList();

            if (listadoEquipo.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipo);
        }

        //agregar
        [HttpPost]
        [Route("add")]
        public IActionResult Crear([FromBody] marcas equiposNuevo)
        {
            try
            {
                _equiposContexto.marcas.Add(equiposNuevo);
                _equiposContexto.SaveChanges();

                return Ok(equiposNuevo);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Modificar
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] marcas equiposModificar)
        {
            marcas? marcaExiste = (from e in _equiposContexto.marcas
                                      where e.id_marcas == id
                                      select e).FirstOrDefault();
            if (marcaExiste == null)
                return NotFound();

            marcaExiste.nombre_marca = equiposModificar.nombre_marca;
            marcaExiste.estados = equiposModificar.estados;
 
            _equiposContexto.Entry(marcaExiste).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(marcaExiste);
        }

        //Eliminar
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult eliminarEquipos(int id)
        {
            marcas? equiposExiste = (from e in _equiposContexto.marcas
                                      where e.id_marcas == id
                                      select e).FirstOrDefault();

            if (equiposExiste == null) return NotFound();
            equiposExiste.estados = "E";
            _equiposContexto.Entry(equiposExiste).State = EntityState.Modified;

           // _equiposContexto.equipos.Attach(equiposExiste);
            //_equiposContexto.equipos.Remove(equiposExiste);
            _equiposContexto.SaveChanges();

            return Ok(equiposExiste);
        }



    }
}
