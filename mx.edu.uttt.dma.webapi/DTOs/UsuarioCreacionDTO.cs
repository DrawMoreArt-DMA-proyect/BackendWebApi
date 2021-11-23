using System;
using System.ComponentModel.DataAnnotations;

namespace mx.edu.uttt.dma.webapi.DTOs
{
    public class UsuarioCreacionDTO
    {
        [Required]
        [StringLength(20)]
        public string UsuarioNombre { get; set; }
        [StringLength(50)]
        public string CorreoElectronico { get; set; }
        [StringLength(100)]
        public string Contrasena { get; set; }
    }
}
