using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P03_WebApi.Models;

namespace P03_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_reservaController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public estados_reservaController(equiposContext equiposContext)
        {
            _equiposContexto = equiposContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            List<estados_reserva> listadoEquipo = (from e in _equiposContexto.estados_reserva
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
        public IActionResult Crear([FromBody] estados_reserva equiposNuevo)
        {
            try
            {
                _equiposContexto.estados_reserva.Add(equiposNuevo);
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
        public IActionResult actualizar(int id, [FromBody] estados_reserva equiposModificar)
        {
            estados_reserva? marcaExiste = (from e in _equiposContexto.estados_reserva
                                            where e.estado_res_id == id
                                   select e).FirstOrDefault();
            if (marcaExiste == null)
                return NotFound();

            marcaExiste.estado = equiposModificar.estado;
            

            _equiposContexto.Entry(marcaExiste).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(marcaExiste);
        }

        //Eliminar
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult eliminarEquipos(int id)
        {
            estados_reserva? equiposExiste = (from e in _equiposContexto.estados_reserva
                                     where e.estado_res_id == id
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
