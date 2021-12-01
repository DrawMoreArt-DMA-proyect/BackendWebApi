using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mx.edu.uttt.dma.webapi.DTOs;
using mx.edu.uttt.dma.webapi.Entidades;
using mx.edu.uttt.dma.webapi.Services;

namespace mx.edu.uttt.dma.webapi.Controllers
{
    [ApiController]
    //Ruta de acceso o url de acceso
    [Route("webapi/logeo")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
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
        //Logeo Usuario
        [HttpPost]
        [Route("login")]
        public ActionResult UserLogin(UsuarioLoginDTO model)
        {
            ActionResult response = Unauthorized();
            var usuario =  _tokenManager.AuthenticateUser(model);
            // var encriptacion = _encriptacionService.Encryptword(model.Contrasena);
            //var existe = await _context.Usuarios.AnyAsync(x =>
            //x.UsuarioNombre == model.UsuarioNombre
            //&& x.Contrasena == encriptacion);
            if (usuario != null)
            {
                //var tokenString = _tokenManager.GenerateJSONWebToken(model);
                response = Ok(new { usuario, Message = "Success" });
            }
            return response;
        }
        //Registro de usuario
        [HttpPost]
        [Route("registro")]
        public ActionResult PostUser(UsuarioCreacionDTO model)
        {
            var encriptacion = _encriptacionService.Encryptword(model.Contrasena);
            var entidad = _mapper.Map<Usuario>(model);
            entidad.Contrasena = encriptacion;
            // entidad.token = "Token Secreto";
            _context.Add(entidad);
            _context.SaveChangesAsync();
            var usuarioDTO = _mapper.Map<UsuarioDTO>(entidad);
            return new CreatedAtRouteResult("obtenerUsuario", new { id = usuarioDTO.IdUsuario }, usuarioDTO);
        }
        /*
        [Authorize]
        [HttpPost("{id:int}")]
        [Route("logout")]
        public ActionResult LogoutUser(int id)
        {

        }
        */
    }
}
