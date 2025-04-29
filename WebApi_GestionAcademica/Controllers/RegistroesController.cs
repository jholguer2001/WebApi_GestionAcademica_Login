using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi_GestionAcademica.Models;

namespace WebApi_GestionAcademica.Controllers
{
    public class RegistroController : ApiController
    {
        private SistemaGestionAcademicaContext db = new SistemaGestionAcademicaContext();

        // GET: api/Registro
        public IQueryable<Registro> GetRegistros()
        {
            return db.Registros;
        }

        // GET: api/Registro/5
        [ResponseType(typeof(Registro))]
        public IHttpActionResult GetRegistro(int id)
        {
            Registro registro = db.Registros.Find(id);
            if (registro == null)
            {
                return NotFound();
            }

            return Ok(registro);
        }

        // PUT: api/Registro/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRegistro(int id, Registro registro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != registro.UsuarioID)
            {
                return BadRequest();
            }

            // Antes de guardar, encriptar la contraseña si se ha cambiado
            // Comprobar si se está actualizando la contraseña comparándola con la base de datos
            var registroExistente = db.Registros.AsNoTracking().FirstOrDefault(r => r.UsuarioID == id);
            if (registroExistente != null && registro.Contrasena != registroExistente.Contrasena)
            {
                // La contraseña ha cambiado, hash antes de guardar
                registro.Contrasena = HashContrasena(registro.Contrasena);
            }

            db.Entry(registro).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Registro
        [ResponseType(typeof(Registro))]
        public IHttpActionResult PostRegistro(Registro registro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar si el usuario ya existe
            if (db.Registros.Any(u => u.Usuario == registro.Usuario))
            {
                return BadRequest("El nombre de usuario ya está en uso.");
            }

            // Verificar si la cédula ya existe
            if (db.Registros.Any(u => u.Cedula == registro.Cedula))
            {
                return BadRequest("La cédula ya está registrada.");
            }

            // Hash de la contraseña antes de guardar
            registro.Contrasena = HashContrasena(registro.Contrasena);

            db.Registros.Add(registro);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = registro.UsuarioID }, registro);
        }

        // DELETE: api/Registro/5
        [ResponseType(typeof(Registro))]
        public IHttpActionResult DeleteRegistro(int id)
        {
            Registro registro = db.Registros.Find(id);
            if (registro == null)
            {
                return NotFound();
            }

            db.Registros.Remove(registro);
            db.SaveChanges();

            return Ok(registro);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RegistroExists(int id)
        {
            return db.Registros.Count(e => e.UsuarioID == id) > 0;
        }

        // Método para hash de contraseña - implementación básica
        private string HashContrasena(string contrasena)
        {
            // En un entorno real, usarías algo como BCrypt.Net o similar
            // Esta es una implementación muy básica solo con fines de ejemplo
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(System.Text.Encoding.UTF8.GetBytes(contrasena)));
        }
    }
}