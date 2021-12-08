using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace mx.edu.uttt.dma.webapi.Services
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        // Obtener ruta del wwwroot para poder colocar archivos ahi
        private readonly IWebHostEnvironment _env;
        // Dominio para construir la url para guardar en BD
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AlmacenadorArchivosLocal(IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task BorrarArchivo(string ruta, string contenedor)
        {
            if(ruta != null)
            {
                var nombreArchivo = Path.GetFileName(ruta);
                string directorioArchivo = Path.Combine(_env.WebRootPath, contenedor, nombreArchivo);

                if (File.Exists(directorioArchivo))
                {
                    File.Delete(directorioArchivo);
                }
            }

            return Task.FromResult(0);
        }

        public async Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string ruta, string contentType)
        {
            await BorrarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenido, extension, contenedor, contentType);
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string contendor, string contentType)
        {
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(_env.WebRootPath, contendor);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string ruta = Path.Combine(folder, nombreArchivo);
            await File.WriteAllBytesAsync(ruta, contenido);

            var urlActual = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var urlParaBD = Path.Combine(urlActual, contendor, nombreArchivo).Replace("\\", "/");
            return urlParaBD;
        }
    }
}
