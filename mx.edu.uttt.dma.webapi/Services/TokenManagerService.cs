using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public TokenManagerService(IConfiguration config,
            ApplicationDbContext context, IEncriptacionService encriptacionService,
            IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
            _encriptacionService = encriptacionService;
            _context = context;
        }
        /*
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEncriptacionService _encriptacionService;
        private readonly ITokenManagerService _tokenManager;

        public LoginController(ApplicationDbContext context,
            IMapper mapper, IEncriptacionService encriptacionService,
            ITokenManagerService tokenManager)
        {
            this._context = context;
            this._mapper = mapper;
            this._encriptacionService = encriptacionService;
            this._tokenManager = tokenManager;
        }
         */
        public UsuarioLoginDTO AuthenticateUser(UsuarioLoginDTO login)
        {
            UsuarioLoginDTO user = null;
            var encriptacion = _encriptacionService.Encryptword(login.Contrasena);
            var usuario = _context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioNombre == login.UsuarioNombre
                                                                   && x.Contrasena == encriptacion);
            
            if (usuario != null)
            {
                //var entidad = _mapper.Map<UsuarioLoginDTO>(usuario);
                user = new UsuarioLoginDTO
                {
                    UsuarioNombre = login.UsuarioNombre
                };
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
