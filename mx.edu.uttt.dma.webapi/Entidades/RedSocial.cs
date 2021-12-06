using System;
using System.ComponentModel.DataAnnotations;

namespace mx.edu.uttt.dma.webapi.Entidades
{
    public class RedSocial
    {
        [Key]
        public int IdRedSocial { get; set; }
        public string Link { get; set; }
        //Relacion de tabla
        public int IdTipoRedSocial { get; set; }
        //public TipoRedesSocial TipoRedesSociales { get; set; }
        //Relacion de Tabla
        public int IdUsuario { get; set; }
        //public Usuario Usuario { get; set; }
    }
}
