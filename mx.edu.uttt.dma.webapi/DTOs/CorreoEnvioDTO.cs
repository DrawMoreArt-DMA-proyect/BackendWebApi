using System;
using System.ComponentModel.DataAnnotations;

namespace mx.edu.uttt.dma.webapi.DTOs
{
    public class CorreoEnvioDTO
    {
        [Required]
        [StringLength(50)]
        public string CorreoElectronico { get; set; }
    }
}
