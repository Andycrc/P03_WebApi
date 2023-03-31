using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P03_WebApi.Models;

namespace P03_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public estados_equipoController(equiposContext equiposContext)
        {
            _equiposContexto = equiposContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            List<estados_equipo> listadoEquipo = (from e in _equiposContexto.estados_equipo
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
        public IActionResult Crear([FromBody] estados_equipo equiposNuevo)
        {
            try
            {
                _equiposContexto.estados_equipo.Add(equiposNuevo);
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
        public IActionResult actualizar(int id, [FromBody] estados_equipo equiposModificar)
        {
            estados_equipo? estados_equipoExiste = (from e in _equiposContexto.estados_equipo
                                           where e.id_estados_equipo == id
                                   select e).FirstOrDefault();
            if (estados_equipoExiste == null)
                return NotFound();

            estados_equipoExiste.descripcion = equiposModificar.descripcion;
            estados_equipoExiste.estado = equiposModificar.estado;

            _equiposContexto.Entry(estados_equipoExiste).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(estados_equipoExiste);
        }

        //Eliminar
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult eliminarEquipos(int id)
        {
            estados_equipo? equiposExiste = (from e in _equiposContexto.estados_equipo
                                             where e.id_estados_equipo == id
                                     select e).FirstOrDefault();

            if (equiposExiste == null) return NotFound();
            equiposExiste.estado = "E";
            _equiposContexto.Entry(equiposExiste).State = EntityState.Modified;

            // _equiposContexto.equipos.Attach(equiposExiste);
            //_equiposContexto.equipos.Remove(equiposExiste);
            _equiposContexto.SaveChanges();

            return Ok(equiposExiste);
        }
    }
}
