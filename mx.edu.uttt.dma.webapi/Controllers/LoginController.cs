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
        public async Task<ActionResult> UserLogin(UsuarioLoginDTO model)
        {
            try
            {
                ActionResult response = Unauthorized();
                // var usuario = _tokenManager.AuthenticateUser(model);
                //if (_tokenManager.AuthenticateUser(model))
                //    return BadRequest("El usuario no existe");

                var encriptacion = _encriptacionService.Encryptword(model.Contrasena);

                var usuario = await _context.Usuarios.FirstOrDefaultAsync
                    (x => x.UsuarioNombre == model.UsuarioNombre
                     && x.Contrasena == encriptacion);

                //var entidadDos = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == 3);
                //_mapper.Map<UsuarioDTO>(entidadDos);

                if (usuario != null)
                {

                    var tokenString = _tokenManager.GenerateJSONWebToken(model);
                    response = Ok(new
                    {
                        IdUsuario = usuario.IdUsuario,
                        UsuarioNombre = usuario.UsuarioNombre,
                        Token = tokenString,
                        Message = "Success"
                    });
                }
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest("Algo a salido Mal");
            }
        }
        //Registro de usuario
        [HttpPost]
        [Route("registro")]
        public ActionResult PostUser(UsuarioCreacionDTO model)
        {
            try
            {
                var encriptacion = _encriptacionService.Encryptword(model.Contrasena);
                var entidad = _mapper.Map<Usuario>(model);
                
                entidad.Contrasena = encriptacion;
                entidad.ImagenPerfil = "https://dmaproyectapi.blob.core.windows.net/perfilimagenes/defaultprofile.jpg";
                //entidad.ImagenPerfil = "default.jpg";
                //entidad.IdSexo = 1;
                // entidad.token = "Token Secreto";
                _context.Add(entidad);
                _context.SaveChangesAsync();
                var usuarioDTO = _mapper.Map<UsuarioDTO>(entidad);
                return Ok("Registro Correcto");
            }
            catch (Exception ex)
            {
                return BadRequest("Algo a salido Mal");
            }
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
