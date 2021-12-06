using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mx.edu.uttt.dma.webapi.DTOs;

namespace mx.edu.uttt.dma.webapi.Services
{
    public interface ITokenManagerService
    {
        public string GenerateJSONWebToken(UsuarioLoginDTO model);
        public Task<ActionResult<UsuarioLoginDTO>> AuthenticateUser(UsuarioLoginDTO login);
    }
}
