using System;
using System.ComponentModel.DataAnnotations;

namespace mx.edu.uttt.dma.webapi.Entidades
{
    public class TipoRedesSociales
    {
        [Key]
        public string IdTipoRedSocial { get; set; }
        public string TipoRedSocial { get; set; }
    }
}
