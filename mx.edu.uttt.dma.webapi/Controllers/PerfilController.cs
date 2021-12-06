using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private readonly string contenedor = "perfilimagenes";

        public PerfilController(ApplicationDbContext context,
            IMapper mapper, IEncriptacionService encriptacionService,
            IAlmacenadorArchivos almacenadorArchivos)
        {
            _context = context;
            _mapper = mapper;
            _encriptacionService = encriptacionService;
            _almacenadorArchivos = almacenadorArchivos;
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
        public async Task<ActionResult> UpdatePerfil(int id,[FromForm]PerfilCreacionDTO model)
        {
            var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);
            if (usuarioDB == null) { return NotFound(); }
            //var existe = await _context.Posts.AnyAsync(x => x.IdPost == id);
            //if (!existe)
            //{
            //    return NotFound();
            //}
            //var entidad = _mapper.Map<Post>(model);
            //entidad.IdPost = id;
            //_context.Entry(entidad).State = EntityState.Modified;

            usuarioDB = _mapper.Map(model, usuarioDB);
            if (model.ImagenPerfil != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.ImagenPerfil.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extencion = Path.GetExtension(model.ImagenPerfil.FileName);
                    usuarioDB.ImagenPerfil = await _almacenadorArchivos.EditarArchivo(contenido, extencion, contenedor,
                        usuarioDB.ImagenPerfil,
                        model.ImagenPerfil.ContentType);
                }
            }
            var encriptacion = _encriptacionService.Encryptword(model.Contrasena);
            usuarioDB.Contrasena = encriptacion;
            await _context.SaveChangesAsync();
            return Ok("Post Actualizado");
            //var encriptacion = _encriptacionService.Encryptword(model.Contrasena);
            //var entidad = _mapper.Map<Usuario>(model);
            //entidad.Contrasena = encriptacion;
            //entidad.IdUsuario = id;
            //_context.Entry(entidad).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            //return Ok("Usuario Actualizado");

        }

        //Eliminar usuario
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var entidad = await _context.Usuarios.FirstOrDefaultAsync(z => z.IdUsuario == id);

            if (!entidad.ImagenPerfil.Equals("https://dmaproyectapi.blob.core.windows.net/perfilimagenes/defaultprofile.jpg"))
            {
                await _almacenadorArchivos.BorrarArchivo(entidad.ImagenPerfil, contenedor);
            }

            if (entidad == null)
            {
                return NotFound();
            }

            _context.Entry(entidad).State = EntityState.Detached;

            _context.Remove(new Usuario() { IdUsuario = id });
            await _context.SaveChangesAsync();

            return Ok("Usuario Eliminado");
        }
    }
}
