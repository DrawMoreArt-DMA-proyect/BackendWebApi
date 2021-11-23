using System;
using System.ComponentModel.DataAnnotations;

namespace mx.edu.uttt.dma.webapi.Entidades
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        [Required]
        [StringLength(20)]
        public string UsuarioNombre { get; set; }
        [StringLength(50)]
        public string CorreoElectronico { get; set; }
        [StringLength(15)]
        public string Contrasena { get; set; }
    }
}
