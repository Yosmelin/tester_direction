using AutoMapper;
using ServicioPAI.Entidades;
using ServicioPAI.Modelos;

namespace ServicioPAI.Servicios
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<PersonaEntity, PersonaResponseDTO>();
        }
    }
}
