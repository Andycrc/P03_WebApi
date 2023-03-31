using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P03_WebApi.Models;

namespace P03_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public reservasController(equiposContext equiposContext)
        {
            _equiposContexto = equiposContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            var listadoReserva = (from reserva in _equiposContexto.reservas
                                  join equipo in _equiposContexto.equipos on reserva.equipo_id equals equipo.id_equipos
                                  join estado in _equiposContexto.estados_reserva on reserva.reserva_id equals estado.estado_res_id
                                  join usuario in _equiposContexto.usuarios on reserva.usuario_id equals usuario.usuario_id
                                  select new
                                  {
                                      nombreEquipo = equipo.nombre,
                                      nombreUsuario = usuario.nombre,
                                      reserva.fecha_salida,
                                      reserva.hora_salida,
                                      reserva.tiempo_reserva,
                                      estado.estado,
                                      reserva.fecha_retorno,
                                      reserva.hora_retorno
                                  }).ToList();

            if (listadoReserva.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoReserva);
        }

        //agregar
        [HttpPost]
        [Route("add")]
        public IActionResult Crear([FromBody] reservas equiposNuevo)
        {
            try
            {
                _equiposContexto.reservas.Add(equiposNuevo);
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
        public IActionResult actualizar(int id, [FromBody] reservas equiposModificar)
        {
            reservas? marcaExiste = (from e in _equiposContexto.reservas
                                     where e.reserva_id == id
                                   select e).FirstOrDefault();
            if (marcaExiste == null)
                return NotFound();

            marcaExiste.fecha_salida = equiposModificar.fecha_salida;
            marcaExiste.hora_salida = equiposModificar.hora_salida;
            marcaExiste.tiempo_reserva = equiposModificar.tiempo_reserva;
            marcaExiste.fecha_retorno = equiposModificar.fecha_retorno;
            marcaExiste.hora_retorno = equiposModificar.hora_retorno;

            _equiposContexto.Entry(marcaExiste).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(marcaExiste);
        }

        //Eliminar
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult eliminarEquipos(int id)
        {
            reservas? carreraExistente = _equiposContexto.reservas.Find(id);

            if (carreraExistente == null) return NotFound();

            _equiposContexto.Entry(carreraExistente).State = EntityState.Deleted;
            _equiposContexto.SaveChanges();

            return Ok(carreraExistente);
        }
    }
}
