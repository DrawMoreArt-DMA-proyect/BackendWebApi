using System;
using System.ComponentModel.DataAnnotations;

namespace mx.edu.uttt.dma.webapi.DTOs
{
    public class PostPatchDTO
    {
        [Required]
        [StringLength(50)]
        public string Titulo { get; set; }
        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }
        //Relacion de Tabla
        public int IdGenero { get; set; }
        //Relacion de Tabla
        public int IdUsuario { get; set; }
    }
}
