using System;
using System.Threading.Tasks;

namespace mx.edu.uttt.dma.webapi.Services
{
    public interface IAlmacenadorArchivos
    {
        public Task<string> GuardarArchivo(byte[] contenido, string extension, string contendor,
            string contentType);
        public Task BorrarArchivo(string ruta, string contenedor);
        public Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor,
            string ruta, string contentType);
    }
}
