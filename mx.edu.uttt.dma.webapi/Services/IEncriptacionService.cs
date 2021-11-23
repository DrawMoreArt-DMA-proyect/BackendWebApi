using System;
namespace mx.edu.uttt.dma.webapi.Services
{
    public interface IEncriptacionService
    {
        public string Encryptword(string Encryptval);
        public string DesEncriptar(string _cadenaAdesencriptar);
    }
}
