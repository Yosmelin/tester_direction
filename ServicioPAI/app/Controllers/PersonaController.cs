using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicioPAI.Entidades;
using ServicioPAI.Modelos;
using ServicioPAI.Servicios;

namespace ServicioPAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly IRepositorioPersonas repositorioPersonas;
        
        public PersonaController(IRepositorioPersonas repositorioPersonas)
        {
            this.repositorioPersonas = repositorioPersonas;           
        }

        [HttpGet]
        [Route("seleccionarPersonaBusquedaAttr")]
        public ActionResult<List<PersonaEntity>> seleccionarPersonaBusquedaAttr(string TipoIdVacunado,
            string NumeroIdVacunado,
            string PrimerNombreVacunado,
            string SegundoNombreVacunado,
            string PrimerApellidoVacunado,
            string SegundoApellidoVacunado,
            string per_parInstitucion,
            DateTime per_FechaNac,
            string TipoIdentificacionMadre,
            string NumeroIdentificacionMadre,
            string PrimerNombreMadre,
            string SegundoNombreMadre,
            string PrimerApellidoMadre,
            string SegundoApellidoMadre,
            int grupoEtareo,
            bool bGetMadre,
            bool bGetHdocum) {

            var personas = repositorioPersonas.seleccionarPersonaBusquedaAttr(TipoIdVacunado,
            NumeroIdVacunado,
            PrimerNombreVacunado,
            SegundoNombreVacunado,
            PrimerApellidoVacunado,
            SegundoApellidoVacunado,
            per_parInstitucion,
            per_FechaNac,
            TipoIdentificacionMadre,
            NumeroIdentificacionMadre,
            PrimerNombreMadre,
            SegundoNombreMadre,
            PrimerApellidoMadre,
            SegundoApellidoMadre,
            grupoEtareo,
            bGetMadre,
            bGetHdocum);

            return personas;
        }

        [HttpGet]
        [Route("seleccionarPersonaIdentificacion")]
        public ActionResult<string> seleccionarPersonaIdentificacion(long per_Consecutivo)
        {
            var personaIdentificacion = repositorioPersonas.seleccionarPersonaIdentificacion(per_Consecutivo);

            return personaIdentificacion;
        }


    }
}
