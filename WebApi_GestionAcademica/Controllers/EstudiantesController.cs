using System;
using System.Linq;
using System.Web.Http;
using System.Net;
using System.Data.Entity;
using WebApi_GestionAcademica.Models;

namespace WebApi_GestionAcademica.Controllers
{
    [RoutePrefix("api/estudiantes")]
    public class EstudiantesController : ApiController
    {
        private SistemaGestionAcademicaContext db = new SistemaGestionAcademicaContext();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetEstudiantes(string filtro = null)
        {
            try
            {
                var query = db.Estudiantes.Include(e => e.Carrera).AsQueryable();

                if (!string.IsNullOrEmpty(filtro))
                {
                    filtro = filtro.ToLower();
                    query = query.Where(e => e.Nombres.ToLower().Contains(filtro)
                                          || e.Apellidos.ToLower().Contains(filtro)
                                          || e.CorreoUTA.ToLower().Contains(filtro)
                                          || e.Cedula.Contains(filtro));
                }

                var estudiantes = query.ToList();

                // Verificación para depurar
                if (estudiantes.Count == 0)
                {
                    // Intenta obtener todos los estudiantes sin filtros ni includes
                    var todosEstudiantes = db.Estudiantes.ToList();
                    if (todosEstudiantes.Count > 0)
                    {
                        return Ok(todosEstudiantes);
                    }
                }

                return Ok(estudiantes);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetEstudiante(int id)
        {
            try
            {
                var estudiante = db.Estudiantes.Include(e => e.Carrera).FirstOrDefault(e => e.EstudianteID == id);
                if (estudiante == null)
                    return NotFound();

                return Ok(estudiante);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult PostEstudiante(Estudiante estudiante)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar cédula
            if (string.IsNullOrEmpty(estudiante.Cedula) || estudiante.Cedula.Length != 10)
                return BadRequest("La cédula debe tener 10 dígitos.");

            // Validar que la cédula sea única
            if (db.Estudiantes.Any(e => e.Cedula == estudiante.Cedula))
                return BadRequest("Ya existe un estudiante con esta cédula.");

            if (!IsValidEmail(estudiante.CorreoUTA))
                return BadRequest("Correo electrónico no válido.");

            if (!db.Carreras.Any(c => c.CarreraID == estudiante.CarreraID))
                return BadRequest("La carrera especificada no existe.");

            // Validar fecha de nacimiento
            if (estudiante.FechaNacimiento > DateTime.Now)
                return BadRequest("La fecha de nacimiento no puede ser futura.");

            db.Estudiantes.Add(estudiante);
            db.SaveChanges();

            return Created($"api/estudiantes/{estudiante.EstudianteID}", estudiante);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult PutEstudiante(int id, Estudiante estudiante)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != estudiante.EstudianteID)
                return BadRequest("El ID del estudiante no coincide.");

            // Validar cédula
            if (string.IsNullOrEmpty(estudiante.Cedula) || estudiante.Cedula.Length != 10)
                return BadRequest("La cédula debe tener 10 dígitos.");

            // Validar que la cédula sea única (excepto para el mismo estudiante)
            if (db.Estudiantes.Any(e => e.Cedula == estudiante.Cedula && e.EstudianteID != id))
                return BadRequest("Ya existe otro estudiante con esta cédula.");

            if (!IsValidEmail(estudiante.CorreoUTA))
                return BadRequest("Correo electrónico no válido.");

            if (!db.Carreras.Any(c => c.CarreraID == estudiante.CarreraID))
                return BadRequest("La carrera especificada no existe.");

            // Validar fecha de nacimiento
            if (estudiante.FechaNacimiento > DateTime.Now)
                return BadRequest("La fecha de nacimiento no puede ser futura.");

            db.Entry(estudiante).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteEstudiante(int id)
        {
            var estudiante = db.Estudiantes.Include(e => e.Asignaciones).FirstOrDefault(e => e.EstudianteID == id);
            if (estudiante == null)
                return NotFound();

            // Eliminar las asignaciones asociadas al estudiante
            foreach (var asignacion in estudiante.Asignaciones.ToList())
            {
                db.Asignaciones.Remove(asignacion);
            }

            // Ahora eliminar el estudiante
            db.Estudiantes.Remove(estudiante);
            db.SaveChanges();

            return Ok(estudiante);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}