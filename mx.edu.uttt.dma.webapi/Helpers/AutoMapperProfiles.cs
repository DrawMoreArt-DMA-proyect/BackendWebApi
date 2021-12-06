using AutoMapper;
using mx.edu.uttt.dma.webapi.DTOs;
using mx.edu.uttt.dma.webapi.Entidades;

namespace mx.edu.uttt.dma.webapi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<UsuarioCreacionDTO, Usuario>();
            CreateMap<UsuarioActualizarDTO, Usuario>();
            CreateMap<Usuario,  UsuarioLoginDTO> ().ReverseMap();
            CreateMap<UsuarioLoginDTO, Usuario>();
            // Perfil
            CreateMap<Usuario, PerfilDTO>().ReverseMap();
            CreateMap<PerfilActualizarPerfil, Usuario>()
                .ForMember(x => x.ImagenPerfil, options => options.Ignore());
            CreateMap<PerfilCreacionDTO, Usuario>();
            // Posts
            CreateMap<Post, PostDTO>().ReverseMap();
            //CreateMap<PostCreacionDTO, Post>();
            CreateMap<PostCreacionDTO, Post>()
                .ForMember(x => x.Imagen, options => options.Ignore());
        }
    }
}
