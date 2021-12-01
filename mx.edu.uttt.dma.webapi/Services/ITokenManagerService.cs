using System;
using mx.edu.uttt.dma.webapi.DTOs;

namespace mx.edu.uttt.dma.webapi.Services
{
    public interface ITokenManagerService
    {
        public string GenerateJSONWebToken(UsuarioLoginDTO model);
        public UsuarioLoginDTO AuthenticateUser(UsuarioLoginDTO login);
    }
}
