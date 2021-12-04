using System;
namespace mx.edu.uttt.dma.webapi.DTOs
{
    public class ResponseDTO
    {
        public string IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Token { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
