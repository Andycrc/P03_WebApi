using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P03_WebApi.Models;

namespace P03_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class carrerasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public carrerasController(equiposContext equiposContext)
        {
            _equiposContexto = equiposContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            var listadoCarreras = (from carreara in _equiposContexto.carreras 
                                   join facultad in _equiposContexto.facultades on carreara.facultad_id equals facultad.facultad_id
                                   select new { 
                                        carreara.nombre_carrera,
                                        facultad.nombre_facultad
                                   }).ToList();

            if (listadoCarreras.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoCarreras);
        }

        //agregar
        [HttpPost]
        [Route("add")]
        public IActionResult Crear([FromBody] carreras equiposNuevo)
        {
            try
            {
                _equiposContexto.carreras.Add(equiposNuevo);
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
        public IActionResult actualizar(int id, [FromBody] carreras equiposModificar)
        {
            carreras? marcaExiste = (from e in _equiposContexto.carreras
                                     where e.carrera_id == id
                                   select e).FirstOrDefault();
            if (marcaExiste == null)
                return NotFound();

            marcaExiste.nombre_carrera = equiposModificar.nombre_carrera;

            _equiposContexto.Entry(marcaExiste).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(marcaExiste);
        }

        //Eliminar
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult eliminarEquipos(int id)
        {
            carreras? carreraExistente = _equiposContexto.carreras.Find(id);

            if (carreraExistente == null) return NotFound();

            _equiposContexto.Entry(carreraExistente).State = EntityState.Deleted;
            _equiposContexto.SaveChanges();

            return Ok(carreraExistente);
        }
    }
}
