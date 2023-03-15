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
            bool bGetHdocum)
        {

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

            if (personas is null)
            {
                return NotFound();
            }

            return personas;
        }


        [HttpGet]
        [Route("SeleccionarPersonaBusquedaId")]
        public ActionResult<PersonaEntity> SeleccionarPersonaBusquedaId(long per_Consecutivo)
        {
            var persona = repositorioPersonas.SeleccionarPersonaBusquedaId(per_Consecutivo);

            if (persona is null)
            {
                return NotFound();
            }

            return persona;
        }


        [HttpGet]
        [Route("seleccionarPersonaIdentificacion/{consecutivo}")]
        public ActionResult<string> seleccionarPersonaIdentificacion(long per_Consecutivo)
        {
            var personaIdentificacion = repositorioPersonas.seleccionarPersonaIdentificacion(per_Consecutivo);

            if (personaIdentificacion is null)
            {
                return NotFound();
            }

            return personaIdentificacion;
        }

        [HttpGet]
        [Route("seleccionarPersonaBusqueda")]
        public IEnumerable<PersonaEntity> seleccionarPersonaBusqueda(string TipoIdVacunado, string NumeroIdVacunado, string PrimerNombreVacunado, string SegundoNombreVacunado, string PrimerApellidoVacunado, string SegundoApellidoVacunado, string per_parInstitucion, DateTime per_FechaNac, string TipoIdentificacionMadre, string NumeroIdentificacionMadre, string PrimerNombreMadre, string SegundoNombreMadre, string PrimerApellidoMadre, string SegundoApellidoMadre, int grupoEtareo)
        {
            return repositorioPersonas.seleccionarPersonaBusqueda(TipoIdVacunado, NumeroIdVacunado, PrimerNombreVacunado, SegundoNombreVacunado, PrimerApellidoVacunado, SegundoApellidoVacunado, per_parInstitucion, per_FechaNac, TipoIdentificacionMadre, NumeroIdentificacionMadre, PrimerNombreMadre, SegundoNombreMadre, PrimerApellidoMadre, SegundoApellidoMadre, grupoEtareo);
        }

        [HttpGet]
        [Route("seleccionarUbicacionPersona")]
        public ActionResult<UbicacionPersonaEntity> seleccionarUbicacionPersona(long per_Consecutivo)
        {
            return repositorioPersonas.seleccionarUbicacionPersona(per_Consecutivo);
        }

        [HttpGet]
        [Route("seleccionarAfiliacionPersona")]
        public ActionResult<PersonaAfiliacionEntity> seleccionarAfiliacionPersona(long per_Consecutivo)
        {
            return repositorioPersonas.seleccionarAfiliacionPersona(per_Consecutivo);
        }

        [HttpGet]
        [Route("seleccionarEstadoContactenos")]
        public string seleccionarEstadoContactenos(long id_Caso)
        {
            return repositorioPersonas.seleccionarEstadoContactenos(id_Caso);
        }

        [HttpGet]
        [Route("seleccionarTablasConNovedades")]

        public List<TablaDominioEntity> seleccionarTablasConNovedades()
        {
            return repositorioPersonas.seleccionarTablasConNovedades();
        }

        [HttpGet]
        [Route("seleccionarTablaDominio")]
        public ActionResult<List<TablaDominioEntity>> seleccionarTablaDominio(short id_Tabla)
        {
            return repositorioPersonas.seleccionarTablaDominio(id_Tabla);

        }

        [HttpPost]
        [Route("insertarUbicacionPersona")]
        public ActionResult<ResultadoConsultaEntity> insertarUbicacionPersona([FromBody] UbicacionPersonaViewDTO ubicacionPersona)
        {
            var resulatado = repositorioPersonas.insertarUbicacionPersona(ubicacionPersona.per_Consecutivo, ubicacionPersona.dir_Direccion, ubicacionPersona.dir_Barrio,
                                                    ubicacionPersona.dir_Codigo_direccion, ubicacionPersona.dir_Upz, ubicacionPersona.dir_CoordenadaX,
                                                    ubicacionPersona.dir_CoordenadaY, ubicacionPersona.dir_Localidad, ubicacionPersona.dir_Estrato,
                                                    ubicacionPersona.tel_Telefono, ubicacionPersona.tel_Contacto, ubicacionPersona.cor_correo,
                                                    ubicacionPersona.dir_mun_id, ubicacionPersona.dir_dep_Id, ubicacionPersona.dir_pais_Id, ubicacionPersona.dir_zon_Id);

            return resulatado;
        }


        [HttpPost]
        [Route("InsertarPersona")]
        public ActionResult<ResultadoConsultaEntity> InsertarPersona([FromBody] PersonaViewDTO insertarPersona)
        {
            var resultado = repositorioPersonas.InsertarPersona(insertarPersona.per_TipoId, insertarPersona.per_Id, insertarPersona.per_CertNacVivo,
                                                    insertarPersona.per_CertDefuncion, insertarPersona.per_TipoIdM, insertarPersona.per_IdM, insertarPersona.per_NumeroHijoM,
                                                    insertarPersona.per_Nombre1M, insertarPersona.per_Nombre2M, insertarPersona.per_Apellido1M, insertarPersona.per_Apellido2M,
                                                    insertarPersona.per_Nombre1, insertarPersona.per_Nombre2, insertarPersona.per_Apellido1, insertarPersona.per_Apellido2,
                                                    insertarPersona.per_FechaNac, insertarPersona.per_Func, insertarPersona.per_Institucion, insertarPersona.per_Estado, insertarPersona.per_cni_id,
                                                    insertarPersona.per_idEtnia, insertarPersona.per_IdGrupoPoblacional, insertarPersona.per_IdGenero, insertarPersona.per_IdGrupoSanguineo, insertarPersona.per_IdRh);

            return resultado;
        }

        [HttpPost]
        [Route("insertarPersonaVacuna")]
        public ActionResult<ResultadoConsultaEntity> insertarPersonaVacuna([FromBody] PersonaVacunaViewDTO personaVacuna)
        {
            var resultado = repositorioPersonas.insertarPersonaVacuna(personaVacuna.per_Id, personaVacuna.per_TipoId,
                                personaVacuna.per_TipoIdM, personaVacuna.per_IdM, personaVacuna.per_NumeroHijoM,
                                personaVacuna.primerNombre, personaVacuna.segundoNombre, personaVacuna.primerApellido, personaVacuna.segundoApellido, personaVacuna.primerNombreM,
                                personaVacuna.segundoNombreM, personaVacuna.primerApellidoM, personaVacuna.segundoApellidoM, personaVacuna.per_ParInstitucion, personaVacuna.cni_id, personaVacuna.etn_idEtnia,
                                personaVacuna.gru_IdGrupo, personaVacuna.per_Genero, personaVacuna.perGrupoSanguineo, personaVacuna.perRh, personaVacuna.cdm_idCondicion, personaVacuna.perFechaNac,
                                personaVacuna.tel_Telefono, personaVacuna.tel_Contacto, personaVacuna.dir_Direccion, personaVacuna.bar_Id, personaVacuna.upz_Id, personaVacuna.loc_id, personaVacuna.dir_mun_id,
                                personaVacuna.dir_dep_Id, personaVacuna.dir_pais_Id, personaVacuna.dir_zon_Id, personaVacuna.cor_correo, personaVacuna.ase_id, personaVacuna.tia_id, personaVacuna.cnv_Id,
                                personaVacuna.vac_IdNoAplicada, personaVacuna.dos_idNoAplicada, personaVacuna.pes_Peso, personaVacuna.vac_Id, personaVacuna.dos_Id, personaVacuna.pse_Id, personaVacuna.com_Id,
                                personaVacuna.cam_id, personaVacuna.vac_FechaVacuna, personaVacuna.vac_actualizacion, personaVacuna.ins_IdVacuna, personaVacuna.vac_Lote, personaVacuna.pos_Id,
                                personaVacuna.per_Func, personaVacuna.per_Institucion);

            return resultado;
        }



        [HttpPost]
        [Route("insertarAfiliacionPersona")]
        public ActionResult<ResultadoConsultaEntity> insertarAfiliacionPersona([FromBody] AfiliacionPersonaViewDTO afiliacionPersona)
        {
            var resultado = repositorioPersonas.insertarAfiliacionPersona(afiliacionPersona.per_Consecutivo,
                                                              afiliacionPersona.ase_id, afiliacionPersona.reg_Id, 
                                                              afiliacionPersona.tia_id);

            return resultado;
        }

        [HttpPost]
        [Route("insertarContactenos")]
        public ActionResult<ResultadoConsultaEntity> insertarContactenos([FromBody] ContactenosViewDTO contactenos)
        {
            var resultado = repositorioPersonas.insertarContactenos(contactenos.con_cat_id,
                                                    contactenos.con_mensaje, contactenos.con_fun_idFunc);

            return resultado;
        }

        [HttpPut]
        [Route("actualizarPersona")]
        public ActionResult<ResultadoConsultaEntity> actualizarPersona(ActualizarPersonaRequestDTO requestDTO)
        {
            var resultado = repositorioPersonas.actualizarPersona(requestDTO);

            return resultado;
        }

    }
}
