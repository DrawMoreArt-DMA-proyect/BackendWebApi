using System;
using System.ComponentModel.DataAnnotations;

namespace mx.edu.uttt.dma.webapi.Entidades
{
    public class Sexos
    {
        [Key]
        public int IdSexo { get; set; }
        public string sexo { get; set; }
    }
}
