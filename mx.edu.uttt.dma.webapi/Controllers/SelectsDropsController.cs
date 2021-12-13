using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mx.edu.uttt.dma.webapi.DTOs;
using mx.edu.uttt.dma.webapi.Services;

namespace mx.edu.uttt.dma.webapi.Controllers
{
    [ApiController]
    //Ruta de acceso o url de acceso
    [Route("webapi/selects")]
    [Authorize]
    public class SelectsDropsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEncriptacionService _encriptacionService;
        public SelectsDropsController(ApplicationDbContext context,
            IMapper mapper, IEncriptacionService encriptacionService)
        {
            this._context = context;
            this._mapper = mapper;
            this._encriptacionService = encriptacionService;
        }

        [HttpGet]
        [Route("generos")]
        public async Task<ActionResult<List<SelectDTO>>> GetGeneros()
        {
            var entidades = await _context.Generos.ToListAsync();
            var dtos = _mapper.Map<List<SelectDTO>>(entidades);
            return dtos;
        }

        [HttpGet]
        [Route("redsocial")]
        public async Task<ActionResult<List<RedSocialDTO>>> GetRedSocial()
        {
            var entidades = await _context.RedesSociales.ToListAsync();
            var dtos = _mapper.Map<List<RedSocialDTO>>(entidades);
            return dtos;
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<List<PostDTO>>> GetPostByUser(int Id)
        {
            try
            {
                //var entidades = await _context.Posts.AnyAsync(x => x.IdUsuario == id);

                //var entidad = await _context.Posts.AnyAsync(x => x.IdUsuario == id);
                var entidades = await _context.Posts.Where(x => x.IdUsuario == Id).ToListAsync();
                if (entidades == null)
                {
                    return NotFound();
                }
                return _mapper.Map<List<PostDTO>>(entidades);

            }
            catch (Exception ex)
            {
                return BadRequest("Algo salio mal");
            }
        }
    }
}
