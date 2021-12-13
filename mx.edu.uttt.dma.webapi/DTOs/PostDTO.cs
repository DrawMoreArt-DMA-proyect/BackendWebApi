using System;
using System.ComponentModel.DataAnnotations;

namespace mx.edu.uttt.dma.webapi.DTOs
{
    public class PostDTO
    {
      
        public int IdPost { get; set; }
        [Required]
        [StringLength(50)]
        public string Titulo { get; set; }
        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }
        //public string MeGusta { get; set; }
        public string Imagen { get; set; }
        //Relacion de Tabla
        public int IdGenero { get; set; }
        public int IdUsuario { get; set; }
    }
}
