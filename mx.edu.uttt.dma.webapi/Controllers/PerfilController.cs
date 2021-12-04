using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("webapi/perfil")]
    //[Authorize]
    public class PerfilController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEncriptacionService _encriptacionService;
        public PerfilController(ApplicationDbContext context,
            IMapper mapper, IEncriptacionService encriptacionService)
        {
            this._context = context;
            this._mapper = mapper;
            this._encriptacionService = encriptacionService;
        }
        //Todos los perfiles
        [HttpGet]
        public async Task<ActionResult<List<PerfilDTO>>> GetPerfiles()
        {
            var entidades = await _context.Usuarios.ToListAsync();
            return _mapper.Map<List<PerfilDTO>>(entidades);
        }
        //Perfil por Id
        [HttpGet("{id:int}", Name = "obtenerPerfil")]
        public async Task<ActionResult<PerfilDTO>> GetPerfilById(int id)
        {
            var entidad = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);

            if (entidad == null)
            {
                return NotFound();
            }
            return _mapper.Map<PerfilDTO>(entidad);
        }
        //Usuario por nombre
        [HttpGet("{usuario}", Name = "obtenerPerfilPorNombre")]
        public async Task<ActionResult<PerfilDTO>> GetPerfilByName(string usuario)
        {
            var entidad = await _context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioNombre.Contains(usuario));
            //.FirstOrDefaultAsync(x => x.UsuarioNombre == usuario);
            if (entidad == null)
            {
                return NotFound();
            }
            return _mapper.Map<PerfilDTO>(entidad);
        }

        //Actualizar todo el usuario
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdatePerfil(int id,[FromForm]PerfilActualizarPerfil model)
        {
            var encriptacion = _encriptacionService.Encryptword(model.Contrasena);
            var entidad = _mapper.Map<Usuario>(model);
            entidad.Contrasena = encriptacion;
            entidad.IdUsuario = id;
            _context.Entry(entidad).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Usuario Actualizado");
        }

        //Eliminar usuario
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var existe = await _context.Usuarios.AnyAsync(x => x.IdUsuario == id);
            if (!existe)
            {
                return NotFound();
            }
            _context.Remove(new Usuario() { IdUsuario = id });
            await _context.SaveChangesAsync();

            return Ok("Usuario Eliminado");
        }
    }
}
