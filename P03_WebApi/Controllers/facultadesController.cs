using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P03_WebApi.Models;

namespace P03_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class facultadesController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public facultadesController(equiposContext equiposContext)
        {
            _equiposContexto = equiposContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            List<facultades> listadoEquipo = (from e in _equiposContexto.facultades
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
        public IActionResult Crear([FromBody] facultades equiposNuevo)
        {
            try
            {
                _equiposContexto.facultades.Add(equiposNuevo);
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
        public IActionResult actualizar(int id, [FromBody] facultades equiposModificar)
        {
            facultades? marcaExiste = (from e in _equiposContexto.facultades
                                       where e.facultad_id == id
                                   select e).FirstOrDefault();
            if (marcaExiste == null)
                return NotFound();

            marcaExiste.nombre_facultad = equiposModificar.nombre_facultad;

            _equiposContexto.Entry(marcaExiste).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(marcaExiste);
        }

        //Eliminar
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult eliminarEquipos(int id)
        {
            facultades? carreraExistente = _equiposContexto.facultades.Find(id);

            if (carreraExistente == null) return NotFound();

            _equiposContexto.Entry(carreraExistente).State = EntityState.Deleted;
            _equiposContexto.SaveChanges();

            return Ok(carreraExistente);
        }
    }
    
}
