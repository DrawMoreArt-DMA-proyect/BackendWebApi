using System;
namespace mx.edu.uttt.dma.webapi.Entidades
{
    public class RedesSociales
    {
        public int IdRedSocial { get; set; }
        public string Link { get; set; }
        //Relacion de tabla
        public int IdTipoRedSocial { get; set; }
        public TipoRedesSociales TipoRedesSociales { get; set; }
        //Relacion de Tabla
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
    }
}
