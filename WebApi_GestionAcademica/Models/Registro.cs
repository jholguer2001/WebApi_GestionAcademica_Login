using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApi_GestionAcademica.Models
{
    [Table("Usuarios")]
    public class Registro
    {
        [Key]
        public int UsuarioID { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "La cédula es requerida")]
        [StringLength(20, ErrorMessage = "La cédula no puede exceder los 20 caracteres")]
        public string Cedula { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres")]
        public string Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder los 50 caracteres")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(255, ErrorMessage = "La contraseña no puede exceder los 255 caracteres")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "El tipo de usuario es requerido")]
        [Range(0, 2, ErrorMessage = "El tipo de usuario debe ser: 0 (Estudiante), 1 (Docente) o 2 (Administrador)")]
        public int Tipo { get; set; }
    }
}