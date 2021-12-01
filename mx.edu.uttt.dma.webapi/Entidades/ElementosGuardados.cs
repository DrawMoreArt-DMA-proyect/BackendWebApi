using System;
using System.ComponentModel.DataAnnotations;

namespace mx.edu.uttt.dma.webapi.Entidades
{
    public class ElementosGuardados
    {
        [Key]
        public int IdElementoGuardado { get; set; }
        //Relacion de Tabla
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        //Relacion de Tabla
        public int IdPost { get; set; }
        public Posts Posts { get; set; }
    }
}
