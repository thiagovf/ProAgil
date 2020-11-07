using System.Linq;
using AutoMapper;
using ProAgil.Domain;
using ProAgil.WebAPI.DTO;

namespace ProAgil.WebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Evento, EventoDTO>()
                .ForMember(destinatarioEventoDTO => destinatarioEventoDTO.Palestrantes, opt => {
                    opt.MapFrom(origemEvento => origemEvento.PalestranteEventos.Select(palestranteEvento => palestranteEvento.Palestrante).ToList());
                });
            CreateMap<Palestrante, PalestranteDTO>()
                .ForMember(destinatarioPalestranteDTO => destinatarioPalestranteDTO.Eventos, opt => {
                    opt.MapFrom(origemPalestrante => origemPalestrante.PalestranteEventos.Select(palestranteEvento => palestranteEvento.Evento).ToList());
                });
            CreateMap<Lote, LoteDTO>();
            CreateMap<RedeSocial, RedeSocialDTO>();
        }
    }
}