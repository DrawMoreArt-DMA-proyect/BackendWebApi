using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        public UsuarioLoginDTO AuthenticateUser(UsuarioLoginDTO login)
        {
            UsuarioLoginDTO user = null;
            var encriptacion = _encriptacionService.Encryptword(login.Contrasena);
            var usuario = _context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioNombre == login.UsuarioNombre
                                                                   && x.Contrasena == encriptacion);
            
            if (usuario != null)
            {
                user = new UsuarioLoginDTO {
                    IdUsuario = login.IdUsuario,
                    UsuarioNombre = login.UsuarioNombre,
                    token = login.token};
            }
            
            return user;
        }

        public string GenerateJSONWebToken(UsuarioLoginDTO model)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, model.UsuarioNombre),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
