using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mx.edu.uttt.dma.webapi.DTOs;
using mx.edu.uttt.dma.webapi.Entidades;
using mx.edu.uttt.dma.webapi.Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace mx.edu.uttt.dma.webapi.Controllers
{
    [ApiController]
    //Ruta de acceso o url de acceso
    [Route("webapi/posts")]
    [Authorize]
    public class PostController : ControllerBase
    {
        // Servicios a Implmentar
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        //private readonly IEncriptacionService _encriptacionService;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        // Nombre de la carpeta en azure
        private readonly string contenedor = "postsimagenes";

        //Contructor de asignacion
        public PostController(ApplicationDbContext context,
            IMapper mapper, IEncriptacionService encriptacionService,
            IAlmacenadorArchivos almacenadorArchivos)
        {
            _context = context;
            _mapper = mapper;
            //_encriptacionService = encriptacionService;
            _almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<List<PostDTO>>> GetUsers()
        {
            var entidades = await _context.Posts.ToListAsync();
            var dtos = _mapper.Map<List<PostDTO>>(entidades);
            return dtos;
        }
        //Post por Id
        [HttpGet("{id:int}", Name = "obtenerPost")]
        public async Task<ActionResult<PostUnoDTO>> GetPostById(int id)
        {
            try
            {
                var entidad = await _context.Posts.FirstOrDefaultAsync(x => x.IdPost == id);

                if (entidad == null)
                {
                    return NotFound();
                }

                return _mapper.Map<PostUnoDTO>(entidad);
            }
            catch (Exception ex)
            {
                return BadRequest("Algo salio mal");
            }
        }

        //[HttpGet("{id:int}", Name = "obtenerPostUsuario")]
        //public async Task<ActionResult<List<PostDTO>>> GetPostByUser(int id)
        //{
        //    try
        //    {
        //        //var entidades = await _context.Posts.AnyAsync(x => x.IdUsuario == id);

        //        //var entidad = await _context.Posts.AnyAsync(x => x.IdUsuario == id);
        //        var entidades = await _context.Posts.Where(x => x.IdUsuario == id).ToListAsync();
        //        if (entidades == null)
        //        {
        //             return NotFound();
        //        }
        //        return _mapper.Map<List<PostDTO>>(entidades);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Algo salio mal");
        //    }
        //}
         
        // Post por titulo
        //[HttpGet("{titulo}", Name = "obtenerNombrePost")]
        //public async Task<ActionResult<PostDTO>> GetUser(string titulo)
        //{
        //    var entidad = await _context.Posts.FirstOrDefaultAsync(x => x.Titulo == titulo);

        //    if (entidad == null)
        //    {
        //        return NotFound();
        //    }

        //    return _mapper.Map<PostDTO>(entidad);
        //}
        // Postear Nuevo Dibujo
        [HttpPost]
        public async Task<ActionResult> PostPosts([FromForm]PostCreacionDTO model)
        {
            var entidad = _mapper.Map<Post>(model);

            if(model.Imagen != null)
            {
                using(var memoryStream =new MemoryStream())
                {
                    await model.Imagen.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extencion = Path.GetExtension(model.Imagen.FileName);
                    entidad.Imagen = await _almacenadorArchivos.GuardarArchivo(contenido, extencion, contenedor,
                        model.Imagen.ContentType);
                }
            }else
            {
                return BadRequest("Imagen de ahuevo");
            }

            _context.Add(entidad);
            await _context.SaveChangesAsync();
            var dto = _mapper.Map<PostDTO>(entidad);
            return new CreatedAtRouteResult("obtenerPost", new { id = entidad.IdPost }, dto);
        }
        // Actualizar Post
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdatePost(int id, [FromForm]PostCreacionDTO model)
        {
            var postDB = await _context.Posts.FirstOrDefaultAsync(x => x.IdPost == id);
            if(postDB == null) { return NotFound(); }
            //var existe = await _context.Posts.AnyAsync(x => x.IdPost == id);
            //if (!existe)
            //{
            //    return NotFound();
            //}
            //var entidad = _mapper.Map<Post>(model);
            //entidad.IdPost = id;
            //_context.Entry(entidad).State = EntityState.Modified;

            postDB = _mapper.Map(model, postDB);
            if (model.Imagen != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.Imagen.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extencion = Path.GetExtension(model.Imagen.FileName);
                    postDB.Imagen = await _almacenadorArchivos.EditarArchivo(contenido, extencion, contenedor,
                        postDB.Imagen,
                        model.Imagen.ContentType);
                }
            }
            await _context.SaveChangesAsync();
            return Ok("Post Actualizado");
        }
            
        // Eliminar post
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            var entidad = await _context.Posts.FirstOrDefaultAsync(x => x.IdPost == id);
            if (entidad == null)
            {
                return NotFound();
            }

            await _almacenadorArchivos.BorrarArchivo(entidad.Imagen,contenedor);

            _context.Entry(entidad).State = EntityState.Detached;

            _context.Remove(new Post() { IdPost = id });
            await _context.SaveChangesAsync();

            return Ok("Post Eliminado");
        }
    }
}
