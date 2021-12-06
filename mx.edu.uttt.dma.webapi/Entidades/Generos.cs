using System;
using System.ComponentModel.DataAnnotations;

namespace mx.edu.uttt.dma.webapi.Entidades
{
    public class Generos
    {
        [Key]
        public int IdGenero { get; set; }
        public string Genero { get; set; }
        public string Descripcion { get; set; }
    }
}
