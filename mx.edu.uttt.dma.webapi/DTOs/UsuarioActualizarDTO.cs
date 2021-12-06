using System;
using System.ComponentModel.DataAnnotations;

namespace mx.edu.uttt.dma.webapi.DTOs
{
    public class UsuarioActualizarDTO
    {
        [Required]
        [StringLength(20)]
        public string UsuarioNombre { get; set; }
        [StringLength(50)]
        public string CorreoElectronico { get; set; }
        [StringLength(15)]
        public string NombrePersona { get; set; }
        [StringLength(100)]
        public string Contrasena { get; set; }
        [StringLength(15)]
        public string ApellidoPaterno { get; set; }
        [StringLength(15)]
        public string ApellidoMaterno { get; set; }
        [StringLength(30)]
        public string FechaDeNacimiento { get; set; }
        [StringLength(500)]
        public string Presentacion { get; set; }
    }
}
