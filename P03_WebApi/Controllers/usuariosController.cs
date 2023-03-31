using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P03_WebApi.Models;

namespace P03_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public usuariosController(equiposContext equiposContext)
        {
            _equiposContexto = equiposContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            var listadoUsuarios = (from usuario in _equiposContexto.usuarios
                                   join carrera in _equiposContexto.carreras on usuario.carrera_id equals carrera.carrera_id
                                   select new
                                   {
                                       usuario.nombre,
                                       usuario.documento,
                                       usuario.tipo,
                                       usuario.carnet,
                                       carrera.nombre_carrera
                                   }).ToList();

            if (listadoUsuarios.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoUsuarios);
        }

        //agregar
        [HttpPost]
        [Route("add")]
        public IActionResult Crear([FromBody] usuarios equiposNuevo)
        {
            try
            {
                _equiposContexto.usuarios.Add(equiposNuevo);
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
        public IActionResult actualizar(int id, [FromBody] usuarios equiposModificar)
        {
            usuarios? marcaExiste = (from e in _equiposContexto.usuarios
                                     where e.usuario_id == id
                                   select e).FirstOrDefault();
            if (marcaExiste == null)
                return NotFound();

            marcaExiste.nombre = equiposModificar.nombre;
            marcaExiste.documento = equiposModificar.documento;
            marcaExiste.tipo = equiposModificar.tipo;
            marcaExiste.carnet = equiposModificar.carnet;

            _equiposContexto.Entry(marcaExiste).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(marcaExiste);
        }

        //Eliminar
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult eliminarEquipos(int id)
        {
            usuarios? carreraExistente = _equiposContexto.usuarios.Find(id);

            if (carreraExistente == null) return NotFound();

            _equiposContexto.Entry(carreraExistente).State = EntityState.Deleted;
            _equiposContexto.SaveChanges();

            return Ok(carreraExistente);
        }
    }
}
