using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using mx.edu.uttt.dma.webapi.DTOs;

namespace mx.edu.uttt.dma.webapi.Services
{
    public class TokenManagerService : ITokenManagerService
    {
        private IConfiguration _config;
        private readonly IEncriptacionService _encriptacionService;
        private readonly ApplicationDbContext _context;

        public TokenManagerService(IConfiguration config,
            ApplicationDbContext context, IEncriptacionService encriptacionService)
        {
            _config = config;
            _encriptacionService = encriptacionService;
            _context = context;
        }
        // Autenticacion de usuario
        public async Task<ActionResult<UsuarioLoginDTO>> AuthenticateUser(UsuarioLoginDTO login)
        {
            UsuarioLoginDTO user = null;
            var encriptacion = _encriptacionService.Encryptword(login.Contrasena);
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioNombre == login.UsuarioNombre
                                                                   && x.Contrasena == encriptacion);
            if (usuario != null)
            {
                return user = new UsuarioLoginDTO
                {
                    IdUsuario = usuario.IdUsuario,
                    UsuarioNombre = usuario.UsuarioNombre
                };
            }
            
            return user;
        }
        // Generador del Token de autenticacion
        public string GenerateJSONWebToken(UsuarioLoginDTO model)
        {
            //var usuario = await _context.Usuarios.FirstOrDefaultAsync
            //    (x => x.UsuarioNombre == model.UsuarioNombre);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, model.UsuarioNombre),
               new Claim(JwtRegisteredClaimNames.Iss, model.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
