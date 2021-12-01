using System;
using System.ComponentModel.DataAnnotations;

namespace mx.edu.uttt.dma.webapi.DTOs
{
    public class UsuarioLoginDTO
    {
        public int IdUsuario { get; set; }
        [Required]
        [StringLength(20)]
        public string UsuarioNombre { get; set; }
        [Required]
        [StringLength(100)]
        public string Contrasena { get; set; }
        public string token { get; set; }
    }
}
