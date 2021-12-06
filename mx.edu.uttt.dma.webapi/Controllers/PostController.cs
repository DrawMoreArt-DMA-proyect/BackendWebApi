using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mx.edu.uttt.dma.webapi.DTOs;
using mx.edu.uttt.dma.webapi.Entidades;
using mx.edu.uttt.dma.webapi.Services;

namespace mx.edu.uttt.dma.webapi.Controllers
{
    [ApiController]
    //Ruta de acceso o url de acceso
    [Route("webapi/posts")]
    //[Authorize]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEncriptacionService _encriptacionService;
        public PostController(ApplicationDbContext context,
            IMapper mapper, IEncriptacionService encriptacionService)
        {
            this._context = context;
            this._mapper = mapper;
            this._encriptacionService = encriptacionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PostDTO>>> GetUsers()
        {
            var entidades = await _context.Posts.ToListAsync();
            var dtos = _mapper.Map<List<PostDTO>>(entidades);
            //Console.WriteLine(dtos);
            return dtos;
        }
        // Post por Id
        [HttpGet("{id:int}", Name = "obtenerPost")]
        public async Task<ActionResult<PostDTO>> GetUserById(int id)
        {
            try
            {
                var entidad = await _context.Posts.FirstOrDefaultAsync(x => x.IdPost == id);

                if (entidad == null)
                {
                    return NotFound();
                }

                return _mapper.Map<PostDTO>(entidad);
            }
            catch (Exception ex)
            {
                return BadRequest("Algo salio mal");
            }
        }
        // Post por titulo
        [HttpGet("{titulo}", Name = "obtenerNombrePost")]
        public async Task<ActionResult<PostDTO>> GetUser(string titulo)
        {
            var entidad = await _context.Posts.FirstOrDefaultAsync(x => x.Titulo == titulo);

            if (entidad == null)
            {
                return NotFound();
            }

            return _mapper.Map<PostDTO>(entidad);
        }
        // Postear Nuevo Dibujo
        [HttpPost]
        public async Task<ActionResult> PostPosts([FromForm]PostCreacionDTO model)
        {
            var entidad = _mapper.Map<Post>(model);
            _context.Add(entidad);
            //await _context.SaveChangesAsync();
            var dto = _mapper.Map<PostDTO>(entidad);
            return new CreatedAtRouteResult("obtenerPost", new { id = entidad.IdPost }, dto);
        }
        // Actualizar Post
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateUser(int id, [FromForm]PostCreacionDTO model)
        {
            var existe = await _context.Posts.AnyAsync(x => x.IdPost == id);

            if (!existe)
            {
                return NotFound();
            }
            var entidad = _mapper.Map<Post>(model);
            entidad.IdPost = id;
            _context.Entry(entidad).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Post Actualizado");
        }
        // Eliminar post
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var existe = await _context.Posts.AnyAsync(x => x.IdPost == id);

            if (!existe)
            {
                return NotFound();
            }
            _context.Remove(new Post() { IdPost = id });
            await _context.SaveChangesAsync();

            return Ok("Post Eliminado");
        }
    }
}
