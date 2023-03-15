
using Directions_Api.Helpers;
using Microsoft.VisualBasic;
using ServicioPAI.Entidades;
using ServicioPAI.Modelos;
using System.Data;
using System.Data.SqlClient;
using static ServicioPAI.Helper.Helper;

namespace ServicioPAI.Servicios
{
    public interface IRepositorioPersonas
    {
        ResultadoConsultaEntity insertarAfiliacionPersona(long per_Consecutivo, string ase_id, int reg_Id, int tia_id);
        ResultadoConsultaEntity insertarContactenos(int con_cat_id, string con_mensaje, string con_fun_idFunc);
        ResultadoConsultaEntity InsertarPersona(string per_TipoId, string per_Id, long per_CertNacVivo, string per_CertDefuncion, string per_TipoIdM, string per_IdM, int per_NumeroHijoM, string per_Nombre1M, string per_Nombre2M, string per_Apellido1M, string per_Apellido2M, string per_Nombre1, string per_Nombre2, string per_Apellido1, string per_Apellido2, DateTime per_FechaNac, string per_Func, string per_Institucion, int per_Estado, int per_cni_id, int per_idEtnia, string per_IdGrupoPoblacional, string per_IdGenero, string per_IdGrupoSanguineo, string per_IdRh); ResultadoConsultaEntity insertarPersonaVacuna(string per_Id, string per_TipoId, string per_TipoIdM, string per_IdM, short per_NumeroHijoM,
            string primerNombre, string segundoNombre, string primerApellido, string segundoApellido, string primerNombreM,
            string segundoNombreM, string primerApellidoM, string segundoApellidoM, string per_ParInstitucion, int cni_id, short etn_idEtnia,
            string gru_IdGrupo, string per_Genero, string perGrupoSanguineo, string perRh, int cdm_idCondicion, DateTime perFechaNac,
            string tel_Telefono, string tel_Contacto, string dir_Direccion, string bar_Id, int upz_Id, int loc_id, int dir_mun_id,
            int dir_dep_Id, string dir_pais_Id, int dir_zon_Id, string cor_correo, string ase_id, int tia_id, int cnv_Id,
            int vac_IdNoAplicada, int dos_idNoAplicada, string pes_Peso, int vac_Id, int dos_Id, int pse_Id, int com_Id,
            int cam_id, DateTime vac_FechaVacuna, bool vac_actualizacion, string ins_IdVacuna, string vac_Lote, int pos_Id,
            string per_Func, string per_Institucion);
        ResultadoConsultaEntity insertarUbicacionPersona(long per_Consecutivo, string dir_Direccion, string dir_Barrio,
            string dir_Codigo_direccion, string dir_Upz, string dir_CoordenadaX, string dir_CoordenadaY,
            int dir_Localidad, string dir_Estrato, string tel_Telefono, string tel_Contacto,
            string cor_correo, int dir_mun_id, int dir_dep_Id, string dir_pais_Id, int dir_zon_Id);
        List<PersonaEntity> seleccionarPersonaBusquedaAttr(string TipoIdVacunado, string NumeroIdVacunado,
            string PrimerNombreVacunado, string SegundoNombreVacunado,
            string PrimerApellidoVacunado, string SegundoApellidoVacunado,
            string per_parInstitucion, DateTime per_FechaNac, string TipoIdentificacionMadre,
            string NumeroIdentificacionMadre, string PrimerNombreMadre, string SegundoNombreMadre,
            string PrimerApellidoMadre, string SegundoApellidoMadre, int grupoEtareo, bool bGetMadre, bool bGetHdocum);
        PersonaEntity SeleccionarPersonaBusquedaId(long per_Consecutivo);
        string seleccionarPersonaIdentificacion(long per_Consecutivo);
        IEnumerable<PersonaEntity> seleccionarPersonaBusqueda(string TipoIdVacunado, string NumeroIdVacunado, string PrimerNombreVacunado, string SegundoNombreVacunado, string PrimerApellidoVacunado, string SegundoApellidoVacunado, string per_parInstitucion, DateTime per_FechaNac, string TipoIdentificacionMadre, string NumeroIdentificacionMadre, string PrimerNombreMadre, string SegundoNombreMadre, string PrimerApellidoMadre, string SegundoApellidoMadre, int grupoEtareo);
        UbicacionPersonaEntity seleccionarUbicacionPersona(long per_Consecutivo);
        PersonaAfiliacionEntity seleccionarAfiliacionPersona(long per_Consecutivo);
        string seleccionarEstadoContactenos(long id_Caso);
        List<TablaDominioEntity> seleccionarTablasConNovedades();
        List<TablaDominioEntity> seleccionarTablaDominio(short id_Tabla);
        ResultadoConsultaEntity actualizarPersona(ActualizarPersonaRequestDTO requestDTO);
    }
    public class RepositorioPersonas : IRepositorioPersonas
    {
        private DateTime fechaBase = Convert.ToDateTime("01/01/1862");
        private readonly string cadena;
        private readonly IRepositorioLog repositorioLog;
        private readonly IRepositorioVacunas repositorioVacunas;

        public RepositorioPersonas(IConfiguration configuration,
            IRepositorioLog repositorioLog, IRepositorioVacunas repositorioVacunas)
        {
            this.cadena = configuration.GetConnectionString("DefaultConnection");
            this.repositorioLog = repositorioLog;
            this.repositorioVacunas = repositorioVacunas;
        }

        public List<PersonaEntity> seleccionarPersonaBusquedaAttr(string TipoIdVacunado,
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
            SqlParameter[] arParms = new SqlParameter[15];

            arParms[0] = new SqlParameter("@TipoIdVacunado", 3);
            if (string.IsNullOrEmpty(TipoIdVacunado))
                arParms[0].Value = DBNull.Value;
            else
                arParms[0].Value = TipoIdVacunado;


            arParms[1] = new SqlParameter("@NumeroIdVacunado", 22);
            if (string.IsNullOrEmpty(NumeroIdVacunado))
                arParms[1].Value = DBNull.Value;
            else
                arParms[1].Value = NumeroIdVacunado;


            arParms[2] = new SqlParameter("@PrimerNombreVacunado", 22);
            if (string.IsNullOrEmpty(PrimerNombreVacunado))
                arParms[2].Value = DBNull.Value;
            else
                arParms[2].Value = PrimerNombreVacunado;


            arParms[3] = new SqlParameter("@SegundoNombreVacunado", 22);
            if (string.IsNullOrEmpty(SegundoNombreVacunado))
                arParms[3].Value = DBNull.Value;
            else
                arParms[3].Value = SegundoNombreVacunado;


            arParms[4] = new SqlParameter("@PrimerApellidoVacunado", 22);
            if (string.IsNullOrEmpty(PrimerApellidoVacunado))
                arParms[4].Value = DBNull.Value;
            else
                arParms[4].Value = PrimerApellidoVacunado;


            arParms[5] = new SqlParameter("@SegundoApellidoVacunado", 22);
            if (string.IsNullOrEmpty(SegundoApellidoVacunado))
                arParms[5].Value = DBNull.Value;
            else
                arParms[5].Value = SegundoApellidoVacunado;


            arParms[6] = new SqlParameter("@per_parInstitucion", 3);
            if (string.IsNullOrEmpty(per_parInstitucion))
                arParms[6].Value = DBNull.Value;
            else
                arParms[6].Value = per_parInstitucion;


            arParms[7] = new SqlParameter("@per_FechaNac", 31);
            arParms[7].Value = per_FechaNac;


            arParms[8] = new SqlParameter("@TipoIdentificacionMadre", 3);
            if (string.IsNullOrEmpty(TipoIdentificacionMadre))
                arParms[8].Value = DBNull.Value;
            else
                arParms[8].Value = TipoIdentificacionMadre;


            arParms[9] = new SqlParameter("@NumeroIdentificacionMadre", 22);
            if (string.IsNullOrEmpty(NumeroIdentificacionMadre))
                arParms[9].Value = DBNull.Value;
            else
                arParms[9].Value = NumeroIdentificacionMadre;


            arParms[10] = new SqlParameter("@PrimerNombreMadre", 22);
            if (string.IsNullOrEmpty(PrimerNombreMadre))
                arParms[10].Value = DBNull.Value;
            else
                arParms[10].Value = PrimerNombreMadre;


            arParms[11] = new SqlParameter("@SegundoNombreMadre", 22);
            if (string.IsNullOrEmpty(SegundoNombreMadre))
                arParms[11].Value = DBNull.Value;
            else
                arParms[11].Value = SegundoNombreMadre;


            arParms[12] = new SqlParameter("@PrimerApellidoMadre", 22);
            if (string.IsNullOrEmpty(PrimerApellidoMadre))
                arParms[12].Value = DBNull.Value;
            else
                arParms[12].Value = PrimerApellidoMadre;


            arParms[13] = new SqlParameter("@SegundoApellidoMadre", 22);
            if (string.IsNullOrEmpty(SegundoApellidoMadre))
                arParms[13].Value = DBNull.Value;
            else
                arParms[13].Value = SegundoApellidoMadre;


            arParms[14] = new SqlParameter("@grupoEtareo", 8);
            arParms[14].Value = grupoEtareo;


            DataSet dsPersona = SqlHelper.ExecuteDataset(cadena, "pa_SeleccionarPersonaBusqueda", arParms);
            List<PersonaEntity> personas = new List<PersonaEntity>();

            foreach (DataRow dr in dsPersona.Tables[0].Rows)
            {
                PersonaEntity persona = new PersonaEntity();

                persona.per_Consecutivo = Convert.ToInt64(dr["per_Consecutivo"]);

                if (!dr["per_TipoId"].GetType().ToString().Equals("System.DBNull"))
                    persona.per_TipoId = dr["per_TipoId"].ToString();


                if (!dr["per_Id"].GetType().ToString().Equals("System.DBNull"))
                    persona.per_Id = dr["per_Id"].ToString();


                if (!dr["per_CertNacVivo"].GetType().ToString().Equals("System.DBNull"))
                    persona.per_CertNacVivo = dr["per_CertNacVivo"].ToString();


                if (!dr["per_CertDefuncion"].GetType().ToString().Equals("System.DBNull"))
                    persona.per_CertDefuncion = dr["per_CertDefuncion"].ToString();


                if (!dr["per_TipoIdM"].GetType().ToString().Equals("System.DBNull"))
                    persona.per_TipoIdM = dr["per_TipoIdM"].ToString();


                if (!dr["per_IdM"].GetType().ToString().Equals("System.DBNull"))
                    persona.per_IdM = dr["per_IdM"].ToString();


                if (!dr["per_NumeroHijoM"].GetType().ToString().Equals("System.DBNull"))
                    persona.per_NumeroHijoM = short.Parse(dr["per_NumeroHijoM"].ToString());


                if (!dr["per_nombre1"].GetType().ToString().Equals("System.DBNull"))
                    persona.primerNombre = (dr["per_nombre1"].ToString());


                if (!dr["per_nombre2"].GetType().ToString().Equals("System.DBNull"))
                    persona.segundoNombre = dr["per_nombre2"].ToString();


                if (!dr["per_apellido1"].GetType().ToString().Equals("System.DBNull"))
                    persona.primerApellido = dr["per_apellido1"].ToString();


                if (!dr["per_apellido2"].GetType().ToString().Equals("System.DBNull"))
                    persona.segundoApellido = dr["per_apellido2"].ToString();


                if (!dr["per_FechaNac"].GetType().ToString().Equals("System.DBNull"))
                    persona.perFechaNac = DateTime.Parse(dr["per_FechaNac"].ToString());


                if (!dr["per_parInstitucion"].GetType().ToString().Equals("System.DBNull"))
                    persona.per_parInstitucion = dr["per_parInstitucion"].ToString();


                if (!dr["per_FechaAlm"].GetType().ToString().Equals("System.DBNull"))
                    persona.per_FechaAlm = DateTime.Parse(dr["per_FechaAlm"].ToString());


                if (!dr["per_func"].GetType().ToString().Equals("System.DBNull"))
                    persona.per_Func = dr["per_func"].ToString();


                if (!dr["per_Estado"].GetType().ToString().Equals("System.DBNull"))
                    persona.per_Estado = int.Parse(dr["per_Estado"].ToString());


                if (!dr["per_Cni_id"].GetType().ToString().Equals("System.DBNull"))
                    persona.cni_id = int.Parse(dr["per_Cni_id"].ToString());


                if (!dr["per_idEtnia"].GetType().ToString().Equals("System.DBNull"))
                    persona.etn_idEtnia = short.Parse(dr["per_idEtnia"].ToString());


                if (!dr["per_IdGrupoPoblacional"].GetType().ToString().Equals("System.DBNull"))
                    persona.gru_IdGrupo = dr["per_IdGrupoPoblacional"].ToString();


                if (!dr["per_IdGenero"].GetType().ToString().Equals("System.DBNull"))
                    persona.per_Genero = dr["per_IdGenero"].ToString();


                if (!dr["per_IdGrupoSanguineo"].GetType().ToString().Equals("System.DBNull"))
                    persona.perGrupoSanguineo = dr["per_IdGrupoSanguineo"].ToString();


                if (!dr["per_IdRh"].GetType().ToString().Equals("System.DBNull"))
                    persona.perRh = dr["per_IdRh"].ToString();


                if (bGetMadre)
                {
                    if (persona.per_TipoIdM != null && persona.per_IdM != null)
                    {
                        List<PersonaEntity> PersonasM = new List<PersonaEntity>();

                        PersonasM = seleccionarPersonaBusquedaAttr(persona.per_TipoIdM, persona.per_IdM, null, null, null, null, null, new DateTime(),
                                                   null, null, null, null, null, null, 3, false, false);

                        if (PersonasM.Count > 0)
                        {
                            persona.primerNombreM = PersonasM[0].primerNombre;
                            persona.segundoNombreM = PersonasM[0].segundoNombre;
                            persona.primerApellidoM = PersonasM[0].primerApellido;
                            persona.segundoApellidoM = PersonasM[0].segundoApellido;
                            persona.per_ConsecutivoM = PersonasM[0].per_Consecutivo;
                        }
                    }
                }


                if (bGetHdocum)
                {
                    string strJson = "{}";
                    persona.hisDocumentJson = seleccionarPersonaIdentificacion(persona.per_Consecutivo);
                }

                personas.Add(persona);
            }


            repositorioLog.insertarLog("seleccionarPersonaBusquedaAttr()", "CORRECTO", arParms);
            repositorioLog.insertarLogBD("seleccionarPersonaBusquedaAttr()", "CORRECTO", arParms);

            return personas;
        }

        public PersonaEntity SeleccionarPersonaBusquedaId(long per_Consecutivo)
        {
            PersonaEntity persona = new PersonaEntity();
            SqlParameter[] arParms = new SqlParameter[1];

            arParms[0] = new SqlParameter("@per_Consecutivo", 0);
            arParms[0].Value = per_Consecutivo;

            DataSet dsPersona = SqlHelper.ExecuteDataset(cadena, "pa_SeleccionarPersonaBusquedaId", arParms);

            if (dsPersona != null && dsPersona.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsPersona.Tables[0].Rows[0];

                persona.per_Consecutivo = per_Consecutivo;

                persona.per_TipoId = DBNull.Value.Equals(dr["per_TipoId"]) ? persona.per_TipoId : Convert.ToString(dr["per_TipoId"]);
                persona.per_Id = DBNull.Value.Equals(dr["per_Id"]) ? persona.per_Id : Convert.ToString(dr["per_Id"]);
                persona.per_CertNacVivo = DBNull.Value.Equals(dr["per_CertNacVivo"]) ? persona.per_CertNacVivo : Convert.ToString(dr["pper_CertNacVivoer_Id"]);
                persona.per_CertDefuncion = DBNull.Value.Equals(dr["per_CertDefuncion"]) ? persona.per_CertDefuncion : Convert.ToString(dr["per_CertDefuncion"]);
                persona.per_TipoIdM = DBNull.Value.Equals(dr["per_TipoIdM"]) ? persona.per_TipoIdM : Convert.ToString(dr["per_TipoIdM"]);
                persona.per_IdM = DBNull.Value.Equals(dr["per_IdM"]) ? persona.per_IdM : Convert.ToString(dr["per_IdM"]);
                persona.per_NumeroHijoM = DBNull.Value.Equals(dr["per_NumeroHijoM"]) ? persona.per_NumeroHijoM : short.Parse(dr["per_NumeroHijoM"].ToString());
                persona.primerNombre = DBNull.Value.Equals(dr["per_Nombre1"]) ? persona.primerNombre : Convert.ToString(dr["per_Nombre1"]);
                persona.segundoNombre = DBNull.Value.Equals(dr["per_Nombre2"]) ? persona.segundoNombre : Convert.ToString(dr["per_Nombre2"]);
                persona.primerApellido = DBNull.Value.Equals(dr["per_Apellido1"]) ? persona.primerApellido : Convert.ToString(dr["per_Apellido1"]);
                persona.segundoApellido = DBNull.Value.Equals(dr["per_Apellido2"]) ? persona.segundoApellido : Convert.ToString(dr["per_Apellido2"]);
                persona.perFechaNac = DBNull.Value.Equals(dr["per_FechaNac"]) ? persona.perFechaNac : Convert.ToDateTime(dr["per_FechaNac"]);
                persona.per_parInstitucion = DBNull.Value.Equals(dr["per_parInstitucion"]) ? persona.per_parInstitucion : Convert.ToString(dr["per_parInstitucion"]);
                persona.per_FechaAlm = DBNull.Value.Equals(dr["per_FechaAlm"]) ? persona.per_FechaAlm : Convert.ToDateTime(dr["per_FechaAlm"]);
                persona.per_Func = DBNull.Value.Equals(dr["per_Func"]) ? persona.per_Func : Convert.ToString(dr["per_Func"]);
                persona.per_Institucion = DBNull.Value.Equals(dr["per_Institucion"]) ? persona.per_Institucion : Convert.ToString(dr["per_Institucion"]);
                persona.per_Estado = DBNull.Value.Equals(dr["per_Estado"]) ? persona.per_Estado : Convert.ToInt32(dr["per_Estado"]);
                persona.cni_id = DBNull.Value.Equals(dr["per_cni_id"]) ? persona.cni_id : Convert.ToInt32(dr["per_cni_id"]);
                persona.etn_idEtnia = DBNull.Value.Equals(dr["per_idEtnia"]) ? persona.etn_idEtnia : short.Parse(dr["per_idEtnia"].ToString());
                persona.gru_IdGrupo = DBNull.Value.Equals(dr["per_IdGrupoPoblacional"]) ? persona.gru_IdGrupo : Convert.ToString(dr["per_IdGrupoPoblacional"]);
                persona.per_Genero = DBNull.Value.Equals(dr["per_IdGenero"]) ? persona.per_Genero : Convert.ToString(dr["per_IdGenero"]);

                persona.perGrupoSanguineo = DBNull.Value.Equals(dr["per_IdGrupoSanguineo"]) ? persona.perGrupoSanguineo : Convert.ToString(dr["per_IdGrupoSanguineo"]);
                persona.perRh = DBNull.Value.Equals(dr["per_IdRh"]) ? persona.perRh : Convert.ToString(dr["per_IdRh"]);
                persona.pes_Peso = DBNull.Value.Equals(dr["Pes_Peso"]) ? persona.pes_Peso : Convert.ToString(dr["Pes_Peso"]);
            }

            dsPersona.Dispose();

            repositorioLog.insertarLog("SeleccionarPersonaBusquedaId()", "CORRECTO", arParms);
            repositorioLog.insertarLogBD("SeleccionarPersonaBusquedaId()", "CORRECTO", arParms);

            return persona;
        }


        public string seleccionarPersonaIdentificacion(long per_Consecutivo)
        {
            string strReturn = "";
            List<SqlParameter> lParms = new List<SqlParameter>();
            var comi = Strings.Chr(34);
            lParms.Add(new SqlParameter("@per_Consecutivo", 0) { Value = per_Consecutivo });


            DataSet dsPersonaIdentificacion = SqlHelper.ExecuteDataset(cadena, "pa_SeleccionarPersonaIdentificacion", lParms.ToArray());

            if (dsPersonaIdentificacion is not null && dsPersonaIdentificacion.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow oRow in dsPersonaIdentificacion.Tables[0].Rows)
                {
                    string strItem = "";
                    strItem += string.Concat(comi, "per_TipoId", comi, ":", comi, oRow["per_TipoId"].ToString(), comi, ",");
                    strItem += string.Concat(comi, "per_Id", comi, ":", comi, oRow["per_Id"].ToString(), comi, "");

                    strReturn += ",{" + strItem + "}";
                }

                if (strReturn != "")
                {
                    strReturn = string.Concat(comi, "historico", comi, ":[", Strings.Replace(strReturn, ",", "", 1, 1, CompareMethod.Binary), "]");
                    strReturn = string.Concat("{", comi, "per_Consecutivo", comi, ":", per_Consecutivo, ",", strReturn, "}");
                }
            }

            SqlParameter[] arParms = new SqlParameter[1];
            arParms[0] = new SqlParameter("@cnv_Id", 3);
            arParms[0].Value = strReturn;

            repositorioLog.insertarLog("seleccionarPersonaIdentificacion()", "CORRECTO", arParms);
            repositorioLog.insertarLogBD("seleccionarPersonaIdentificacion()", "CORRECTO", arParms);

            return strReturn;
        }


        public int seleccionarPersonaIdContar(string per_Id, string per_TipoId)
        {
            PersonaEntity oPersona = new PersonaEntity();

            SqlParameter[] arParms = new SqlParameter[1];
            arParms[0] = new SqlParameter("@per_TipoId", 0);
            arParms[0].Value = per_TipoId;
            arParms[1] = new SqlParameter("@per_Id", 0);
            arParms[1].Value = per_Id;

            int nroPersonas = Convert.ToInt32(SqlHelper.ExecuteScalar(cadena, "pa_SeleccionarPersonaIdContar", arParms));

            repositorioLog.insertarLog("seleccionarPersonaIdContar()", "CORRECTO", arParms);
            repositorioLog.insertarLog("seleccionarPersonaIdContar()", "CORRECTO", arParms);

            return nroPersonas;
        }

        public ResultadoConsultaEntity insertarUbicacionPersona(long per_Consecutivo, string dir_Direccion, string dir_Barrio,
            string dir_Codigo_direccion, string dir_Upz, string dir_CoordenadaX, string dir_CoordenadaY, int dir_Localidad,
            string dir_Estrato, string tel_Telefono, string tel_Contacto, string cor_correo, int dir_mun_id, int dir_dep_Id,
            string dir_pais_Id, int dir_zon_Id)
        {
            ResultadoConsultaEntity oResultado = new ResultadoConsultaEntity();
            bool fValidacion = true;

            if (per_Consecutivo == 0)
            {
                oResultado.resultado = false;
                oResultado.errores = "La variable per_Consecutivo no puede ser 0";
                fValidacion = false;
            }

            if (string.IsNullOrEmpty(dir_Direccion))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable dir_Direccion no puede estar vacía";
                fValidacion = false;
            }

            if (fValidacion)
            {
                SqlParameter[] arParms = new SqlParameter[16];

                arParms[0] = new SqlParameter("@per_Consecutivo", 0);
                arParms[0].Value = per_Consecutivo;

                arParms[1] = new SqlParameter("@dir_Direccion", 22);
                if (string.IsNullOrEmpty(dir_Direccion))
                    arParms[1].Value = DBNull.Value;
                else
                    arParms[1].Value = dir_Direccion;


                arParms[2] = new SqlParameter("@dir_Barrio", 22);
                if (string.IsNullOrEmpty(dir_Barrio))
                    arParms[2].Value = DBNull.Value;
                else
                    arParms[2].Value = dir_Barrio;

                arParms[3] = new SqlParameter("@dir_codigo_direccion", 22);
                if (string.IsNullOrEmpty(dir_Codigo_direccion))
                    arParms[3].Value = DBNull.Value;
                else
                    arParms[3].Value = dir_Codigo_direccion;

                arParms[4] = new SqlParameter("@dir_Upz", 22);
                if (string.IsNullOrEmpty(dir_Upz) || dir_Upz.Equals("0"))
                    arParms[4].Value = DBNull.Value;
                else
                    arParms[4].Value = dir_Upz;

                arParms[5] = new SqlParameter("@dir_CoordenadaX", 22);
                if (string.IsNullOrEmpty(dir_CoordenadaX))
                    arParms[5].Value = DBNull.Value;
                else
                    arParms[5].Value = dir_CoordenadaX;

                arParms[6] = new SqlParameter("@dir_CoordenadaY", 22);
                if (string.IsNullOrEmpty(dir_CoordenadaY))
                    arParms[6].Value = DBNull.Value;
                else
                    arParms[6].Value = dir_CoordenadaY;

                arParms[7] = new SqlParameter("@dir_Localidad", 8);
                if (dir_Localidad == 0)
                    arParms[7].Value = DBNull.Value;
                else
                    arParms[7].Value = dir_Localidad;

                arParms[8] = new SqlParameter("@dir_Estrato", 8);
                if (string.IsNullOrEmpty(dir_Estrato) || dir_Upz.Equals("0"))
                    arParms[8].Value = DBNull.Value;
                else
                    arParms[8].Value = dir_Estrato;

                arParms[9] = new SqlParameter("@tel_Telefono", 22);
                if (string.IsNullOrEmpty(tel_Telefono))
                    arParms[9].Value = DBNull.Value;
                else
                    arParms[9].Value = tel_Telefono;

                arParms[10] = new SqlParameter("@tel_Contacto", 22);
                if (string.IsNullOrEmpty(tel_Contacto))
                    arParms[10].Value = DBNull.Value;
                else
                    arParms[10].Value = tel_Contacto;

                arParms[11] = new SqlParameter("@cor_correo", 22);
                if (string.IsNullOrEmpty(cor_correo))
                    arParms[11].Value = DBNull.Value;
                else
                    arParms[11].Value = cor_correo;

                arParms[12] = new SqlParameter("@dir_mun_id", 8);
                if (string.IsNullOrEmpty(Convert.ToString(dir_mun_id)) || dir_mun_id == 0)
                    arParms[12].Value = DBNull.Value;
                else
                    arParms[12].Value = dir_mun_id;

                arParms[13] = new SqlParameter("@dir_dep_Id", 8);
                if (string.IsNullOrEmpty(Convert.ToString(dir_dep_Id)) || dir_dep_Id == 0)
                    arParms[13].Value = DBNull.Value;
                else
                    arParms[13].Value = dir_dep_Id;

                arParms[14] = new SqlParameter("@dir_pais_Id", 3);
                if (string.IsNullOrEmpty(dir_pais_Id))
                    arParms[14].Value = DBNull.Value;
                else
                    arParms[14].Value = dir_pais_Id;

                arParms[15] = new SqlParameter("@dir_zon_Id", 20);
                if (string.IsNullOrEmpty(Convert.ToString(dir_zon_Id)) || dir_zon_Id == 0)
                    arParms[15].Value = DBNull.Value;
                else
                    arParms[15].Value = dir_zon_Id;


                try
                {
                    if (SqlHelper.ExecuteNonQuery(cadena, "pa_InsertarUbicacionPersonaWS", arParms) > 0)
                    {
                        oResultado.resultado = true;
                        oResultado.errores = string.Empty;
                        oResultado.consecutivo = Convert.ToString(per_Consecutivo);

                        repositorioLog.insertarLog("insertarUbicacionPersona()", "CORRECTO", arParms);
                        repositorioLog.insertarLogBD("insertarUbicacionPersona()", "CORRECTO", arParms);
                    }
                    else
                    {
                        oResultado.resultado = false;
                        oResultado.errores = "Registro no identificado.";

                        repositorioLog.insertarLog("insertarUbicacionPersona()", "No se inserto ninguna ubicacion", arParms);
                        repositorioLog.insertarLogBD("insertarUbicacionPersona()", "No se inserto ninguna ubicacion", arParms);
                    }
                }
                catch (SqlException ex)
                {
                    string errorMessage = ex.Message;
                    int errorCode = ex.ErrorCode;
                    oResultado.resultado = false;
                    oResultado.errores = errorMessage;

                    repositorioLog.insertarLog("insertarUbicacionPersona()", ex.Message, arParms);
                    repositorioLog.insertarLogBD("insertarUbicacionPersona()", ex.Message, arParms);
                }

            }

            return oResultado;
        }


        public ResultadoConsultaEntity insertarAfiliacionPersona(long per_Consecutivo, string ase_id, int reg_Id, int tia_id)
        {
            ResultadoConsultaEntity oResultado = new ResultadoConsultaEntity();
            bool fValidacion = true;
            string MensajeValidacion = string.Empty;

            if (per_Consecutivo == 0)
            {
                oResultado.resultado = false;
                oResultado.errores = " La variable per_Consecutivo no puede ser 0";
                fValidacion = false;
            }

            if (string.IsNullOrEmpty(ase_id))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable ase_id no puede estar vacía";
                fValidacion = false;
            }

            if (tia_id == 0)
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable tia_id no puede ser 0";
                fValidacion = false;
            }

            //----------------- Aqui se valida contra las tablas de dominio
            if (!(validarTablasDominio(1, ase_id, MensajeValidacion)))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }

            if (!(validarTablasDominio(2, tia_id.ToString(), MensajeValidacion)))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }

            if (fValidacion)
            {
                SqlParameter[] arParms = new SqlParameter[4];

                arParms[0] = new SqlParameter("@per_Consecutivo", 0);
                arParms[0].Value = per_Consecutivo;

                arParms[1] = new SqlParameter("@ase_id", 22);
                arParms[1].Value = ase_id;

                arParms[2] = new SqlParameter("@reg_Id", 0);
                arParms[2].Value = reg_Id;

                arParms[3] = new SqlParameter("@tia_id", 0);
                arParms[3].Value = tia_id;

                try
                {
                    if (SqlHelper.ExecuteNonQuery(cadena, "pa_InsertarAseguradoraPersonaWS", arParms) > 0)
                    {
                        oResultado.resultado = true;
                        oResultado.errores = string.Empty;
                        oResultado.consecutivo = Convert.ToString(per_Consecutivo);

                        repositorioLog.insertarLog("insertarAfiliacionPersona()", "CORRECTO", arParms);
                        repositorioLog.insertarLogBD("insertarAfiliacionPersona()", "CORRECTO", arParms);
                    }
                    else
                    {
                        oResultado.resultado = false;
                        oResultado.errores = "Registro no identificado.";

                        repositorioLog.insertarLog("insertarAfiliacionPersona()", "No realizo la insercion", arParms);
                        repositorioLog.insertarLogBD("insertarAfiliacionPersona()", "No realizo la insercion", arParms);
                    }
                }
                catch (SqlException ex)
                {
                    string errorMessage = ex.Message;
                    int errorCode = ex.ErrorCode;
                    oResultado.resultado = false;
                    oResultado.errores = errorMessage;

                    repositorioLog.insertarLog("insertarAfiliacionPersona()", ex.Message, arParms);
                    repositorioLog.insertarLogBD("insertarAfiliacionPersona()", ex.Message, arParms);
                }
            }

            return oResultado;
        }

        public ResultadoConsultaEntity insertarContactenos(int con_cat_id, string con_mensaje, string con_fun_idFunc)
        {
            ResultadoConsultaEntity oResultado = new ResultadoConsultaEntity();

            SqlParameter[] arParms = new SqlParameter[3];

            arParms[0] = new SqlParameter("@con_cat_id", 16);
            arParms[0].Value = con_cat_id;

            arParms[1] = new SqlParameter("@con_mensaje", 22);
            arParms[1].Value = con_mensaje;

            arParms[2] = new SqlParameter("@con_fun_idFunc", 12);
            arParms[2].Value = con_fun_idFunc;

            try
            {
                var idResult = (SqlHelper.ExecuteScalar(cadena, "pa_InsertarContactenos_getId", arParms));
                idResult = DBNull.Value.Equals(idResult) ? -1 : Convert.ToInt64(idResult);

                if (Convert.ToUInt64(idResult) > 0)
                {
                    oResultado.resultado = true;
                    oResultado.errores = string.Empty;
                    oResultado.consecutivo = Convert.ToString(idResult);

                    repositorioLog.insertarLog("insertarContactenos()", "CORRECTO", arParms);
                }
                else
                {
                    oResultado.resultado = false;
                    oResultado.errores = "Registro no ingresado.";

                    repositorioLog.insertarLog("insertarContactenos()", "No inserto ningun registro", arParms);
                }
            }
            catch (Exception ex)
            {
                oResultado.resultado = false;
                oResultado.errores = ex.Message.ToString();
                repositorioLog.insertarLog("insertarContactenos()", ex.Message, arParms);
            }

            return oResultado;
        }

        public ResultadoConsultaEntity InsertarPersona(string per_TipoId, string per_Id,
            long per_CertNacVivo,
            string per_CertDefuncion,
            string per_TipoIdM,
            string per_IdM,
            int per_NumeroHijoM,
            string per_Nombre1M,
            string per_Nombre2M,
            string per_Apellido1M,
            string per_Apellido2M,
            string per_Nombre1,
            string per_Nombre2,
            string per_Apellido1,
            string per_Apellido2,
            DateTime per_FechaNac,
            string per_Func,
            string per_Institucion,
            int per_Estado,
            int per_cni_id,
            int per_idEtnia,
            string per_IdGrupoPoblacional,
            string per_IdGenero,
            string per_IdGrupoSanguineo,
            string per_IdRh)
        {
            ResultadoConsultaEntity oResultado = new ResultadoConsultaEntity();

            bool fValidacion = true;
            string MensajeValidacion = string.Empty;

            //---------------Aqui se validan los valores en 0 o vacios
            if (string.IsNullOrEmpty(per_TipoId))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_tipoId no puede estar vacía";
            }

            if (string.IsNullOrEmpty(per_Id))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_Id no puede estar vacía";
            }

            if (string.IsNullOrEmpty(per_Nombre1))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_Nombre1 no puede estar vacía";
            }

            if (string.IsNullOrEmpty(per_Apellido1))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_Apellido1 no puede estar vacía";
            }

            if (per_FechaNac == DateTime.MinValue)
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_FechaNac no puede estar vacía";
            }
            else
            {
                decimal edad = calcularEdad(per_FechaNac);

                if (edad < Constantes.MAYOR_EDAD)
                {
                    if (string.IsNullOrEmpty(per_TipoIdM))
                    {
                        oResultado.resultado = false;
                        oResultado.errores += " La variable per_TipoIdM no puede estar vacía";
                    }

                    if (string.IsNullOrEmpty(per_IdM))
                    {
                        oResultado.resultado = false;
                        oResultado.errores += " La variable per_IdM no puede estar vacía";
                    }

                    if (string.IsNullOrEmpty(per_Nombre1M))
                    {
                        oResultado.resultado = false;
                        oResultado.errores += " La variable per_Nombre1M no puede estar vacía";
                    }

                    if (string.IsNullOrEmpty(per_Apellido1M))
                    {
                        oResultado.resultado = false;
                        oResultado.errores += " La variable per_Apellido1M no puede estar vacía";
                    }

                    if (!(validarTablasDominio(14, per_TipoIdM, MensajeValidacion)))
                    {
                        oResultado.resultado = false;
                        oResultado.errores += MensajeValidacion;
                    }
                }
            }

            if (string.IsNullOrEmpty(per_Func))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_Func no puede estar vacía";
            }

            if (string.IsNullOrEmpty(per_Institucion))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_Institucion no puede estar vacía";
            }

            if (per_Estado != Constantes.ESTADO_ACTIVO || per_Estado != Constantes.ESTADO_FALLECIDO)
            {
                oResultado.resultado = false;
                oResultado.errores += "La variable per_Estado debe ser 2 (Registro activo) o 4 (Fallecido)";
                fValidacion = false;
            }

            if (per_idEtnia == 0)
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_idEtnia no puede estar vacía";
            }

            if (string.IsNullOrEmpty(per_IdGenero))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_IdGenero no puede estar vacía";
            }

            //----------------- Aqui se valida contra las tablas de dominio
            if (!(validarTablasDominio(14, per_TipoId, MensajeValidacion)))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }

            if (!(validarTablasDominio(32, per_Func.ToString(), MensajeValidacion)))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }

            if (!(validarTablasDominio(15, per_Institucion.ToString(), MensajeValidacion)))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }

            if (!(validarTablasDominio(16, per_Estado.ToString(), MensajeValidacion)))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }

            if (per_cni_id != 0)
            {
                if (!(validarTablasDominio(17, per_cni_id.ToString(), MensajeValidacion)))
                {
                    oResultado.resultado = false;
                    oResultado.errores += MensajeValidacion;
                }
            }

            if (!(validarTablasDominio(18, per_idEtnia.ToString(), MensajeValidacion)))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }


            if (!string.IsNullOrEmpty(per_IdGrupoPoblacional))
            {
                if (!(validarTablasDominio(19, per_IdGrupoPoblacional, MensajeValidacion)))
                {
                    oResultado.resultado = false;
                    oResultado.errores += MensajeValidacion;
                }
            }

            if (!(validarTablasDominio(20, per_IdGenero, MensajeValidacion)))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }

            if (!string.IsNullOrEmpty(per_IdGrupoSanguineo))
            {
                if (!(validarTablasDominio(20, per_IdGrupoSanguineo, MensajeValidacion)))
                {
                    oResultado.resultado = false;
                    oResultado.errores += MensajeValidacion;
                }
            }

            if (!string.IsNullOrEmpty(per_IdRh))
            {
                if (!(validarTablasDominio(22, per_IdRh, MensajeValidacion)))
                {
                    oResultado.resultado = false;
                    oResultado.errores += MensajeValidacion;
                }
            }


            //-------------------
            if (fValidacion)
            {
                SqlParameter[] arParms = new SqlParameter[25];

                arParms[0] = new SqlParameter("@per_TipoId", 3);
                if (string.IsNullOrEmpty(per_TipoId))
                    arParms[0].Value = DBNull.Value;
                else
                    arParms[0].Value = per_TipoId;

                arParms[1] = new SqlParameter("@per_Id", 22);
                if (string.IsNullOrEmpty(per_Id))
                    arParms[1].Value = DBNull.Value;
                else
                    arParms[1].Value = per_Id;

                arParms[2] = new SqlParameter("@per_CertNacVivo", 0);
                if (per_CertNacVivo == 0)
                    arParms[2].Value = DBNull.Value;
                else
                    arParms[2].Value = per_CertNacVivo;

                arParms[3] = new SqlParameter("@per_CertDefuncion", 22);
                if (string.IsNullOrEmpty(per_CertDefuncion))
                    arParms[3].Value = DBNull.Value;
                else
                    arParms[3].Value = per_CertDefuncion;

                arParms[4] = new SqlParameter("@per_TipoIdM", 3);
                if (string.IsNullOrEmpty(per_TipoIdM))
                    arParms[4].Value = DBNull.Value;
                else
                    arParms[4].Value = per_TipoIdM;

                arParms[5] = new SqlParameter("@per_IdM", 22);
                if (string.IsNullOrEmpty(per_IdM))
                    arParms[5].Value = DBNull.Value;
                else
                    arParms[5].Value = per_IdM;

                arParms[6] = new SqlParameter("@per_NumeroHijoM", 16);
                if (per_NumeroHijoM == 0)
                    arParms[6].Value = DBNull.Value;
                else
                    arParms[6].Value = per_NumeroHijoM;

                arParms[7] = new SqlParameter("@per_Nombre1M", 22);
                if (string.IsNullOrEmpty(per_Nombre1M))
                    arParms[7].Value = DBNull.Value;
                else
                    arParms[7].Value = per_Nombre1M;

                arParms[8] = new SqlParameter("@per_Nombre2M", 22);
                if (string.IsNullOrEmpty(per_Nombre2M))
                    arParms[8].Value = DBNull.Value;
                else
                    arParms[8].Value = per_Nombre2M;

                arParms[9] = new SqlParameter("@per_Apellido1M", 22);
                if (string.IsNullOrEmpty(per_Apellido1M))
                    arParms[9].Value = DBNull.Value;
                else
                    arParms[9].Value = per_Apellido1M;

                arParms[10] = new SqlParameter("@per_Apellido2M", 22);
                if (string.IsNullOrEmpty(per_Apellido2M))
                    arParms[10].Value = DBNull.Value;
                else
                    arParms[10].Value = per_Apellido2M;

                arParms[11] = new SqlParameter("@per_Nombre1", 22);
                if (string.IsNullOrEmpty(per_Nombre1))
                    arParms[11].Value = DBNull.Value;
                else
                    arParms[11].Value = per_Nombre1;

                arParms[12] = new SqlParameter("@per_Nombre2", 22);
                if (string.IsNullOrEmpty(per_Nombre2))
                    arParms[12].Value = DBNull.Value;
                else
                    arParms[12].Value = per_Nombre2;

                arParms[13] = new SqlParameter("@per_Apellido1", 22);
                if (string.IsNullOrEmpty(per_Apellido1))
                    arParms[13].Value = DBNull.Value;
                else
                    arParms[13].Value = per_Apellido1;

                arParms[14] = new SqlParameter("@per_Apellido2", 22);
                if (string.IsNullOrEmpty(per_Apellido2))
                    arParms[14].Value = DBNull.Value;
                else
                    arParms[14].Value = per_Apellido2;

                arParms[15] = new SqlParameter("@per_FechaNac", 31);
                if (per_FechaNac == DateTime.MinValue)
                    arParms[15].Value = DBNull.Value;
                else
                    arParms[15].Value = per_FechaNac;

                arParms[16] = new SqlParameter("@per_Func", 3);
                if (string.IsNullOrEmpty(per_Func))
                    arParms[16].Value = DBNull.Value;
                else
                    arParms[16].Value = per_Func;

                arParms[17] = new SqlParameter("@per_Institucion", 12);
                if (string.IsNullOrEmpty(per_Institucion))
                    arParms[17].Value = DBNull.Value;
                else
                    arParms[17].Value = per_Institucion;

                arParms[18] = new SqlParameter("@per_Estado", 8);
                if (per_Estado == 0)
                    arParms[18].Value = DBNull.Value;
                else
                    arParms[18].Value = per_Estado;

                arParms[19] = new SqlParameter("@per_cni_id", 8);
                if (per_cni_id == 0)
                    arParms[19].Value = DBNull.Value;
                else
                    arParms[19].Value = per_cni_id;

                arParms[20] = new SqlParameter("@per_idEtnia", 22);
                if (per_idEtnia == 0)
                    arParms[20].Value = DBNull.Value;
                else
                    arParms[20].Value = per_idEtnia;

                arParms[21] = new SqlParameter("@per_IdGrupoPoblacional", 3);
                if (per_IdGrupoPoblacional == "0")
                    arParms[21].Value = DBNull.Value;
                else
                    arParms[21].Value = per_IdGrupoPoblacional;

                arParms[22] = new SqlParameter("@per_IdGenero", 3);
                if (string.IsNullOrEmpty(per_IdGenero))
                    arParms[22].Value = DBNull.Value;
                else
                    arParms[22].Value = per_IdGenero;

                arParms[23] = new SqlParameter("@per_IdGrupoSanguineo", 3);
                if (string.IsNullOrEmpty(per_IdGrupoSanguineo))
                    arParms[23].Value = DBNull.Value;
                else
                    arParms[23].Value = per_IdGrupoSanguineo;

                arParms[24] = new SqlParameter("@per_IdRh", 3);
                if (string.IsNullOrEmpty(per_IdRh))
                    arParms[24].Value = DBNull.Value;
                else
                    arParms[24].Value = per_IdRh;

                arParms[25] = new SqlParameter("@per_Consecutivo", 0);
                arParms[25].Value = DBNull.Value;

                try
                {
                    long consec = Convert.ToInt64(SqlHelper.ExecuteScalar(cadena, "pa_InsertarPersonaWS", arParms));

                    if (consec > 0)
                    {
                        oResultado.consecutivo = Convert.ToString(consec);
                        oResultado.resultado = true;
                        oResultado.errores = string.Empty;

                        repositorioLog.insertarLog("insertarPersona()", "CORRECTO", arParms);
                        repositorioLog.insertarLogBD("insertarPersona()", "El registro no fue ingresado", arParms);
                    }
                }
                catch (SqlException e)
                {
                    string errorMessage = e.Message;
                    int errorCode = e.ErrorCode;
                    oResultado.resultado = false;
                    oResultado.errores = errorMessage;

                    repositorioLog.insertarLog("insertarPersona()", e.Message, arParms);
                    repositorioLog.insertarLogBD("insertarPersona()", e.Message, arParms);
                }
            }

            return oResultado;
        }


        public ResultadoConsultaEntity insertarPersonaVacuna(string per_Id, string per_TipoId, string per_TipoIdM, string per_IdM,
                     short per_NumeroHijoM, string primerNombre, string segundoNombre, string primerApellido, string segundoApellido,
                     string primerNombreM, string segundoNombreM, string primerApellidoM, string segundoApellidoM, string per_ParInstitucion,
                     int cni_id, short etn_idEtnia, string gru_IdGrupo, string per_Genero, string perGrupoSanguineo, string perRh,
                     int cdm_idCondicion, DateTime perFechaNac, string tel_Telefono, string tel_Contacto, string dir_Direccion, string bar_Id,
                     int upz_Id, int loc_id, int dir_mun_id, int dir_dep_Id, string dir_pais_Id, int dir_zon_Id, string cor_correo, string ase_id,
                     int tia_id, int cnv_Id, int vac_IdNoAplicada, int dos_idNoAplicada, string pes_Peso, int vac_Id, int dos_Id, int pse_Id,
                     int com_Id, int cam_id, DateTime vac_FechaVacuna, bool vac_actualizacion, string ins_IdVacuna, string vac_Lote, int pos_Id,
                     string per_Func, string per_Institucion)
        {
            ResultadoConsultaEntity eResultadoConsulta = new ResultadoConsultaEntity();
            PersonaEntity ePersona = new PersonaEntity();

            bool fValidacion = true;
            string MensajeValidacion = string.Empty;

            //---------------Aqui se validan los valores en 0 o vacios

            if (string.IsNullOrEmpty(per_Id))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable per_Id no puede estar vacía";
                fValidacion = false;
            }

            if (string.IsNullOrEmpty(per_TipoId))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable per_TipoId no puede estar vacía";
                fValidacion = false;
            }

            if (string.IsNullOrEmpty(per_TipoIdM))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable per_TipoIdM no puede estar vacía";
                fValidacion = false;
            }

            if (string.IsNullOrEmpty(per_IdM))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable per_IdM no puede estar vacía";
                fValidacion = false;
            }

            if (per_NumeroHijoM == 0)
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable per_NumeroHijoM no puede ser 0";
                fValidacion = false;
            }

            if (string.IsNullOrEmpty(primerNombre))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable primerNombre no puede estar vacía";
                fValidacion = false;
            }

            if (string.IsNullOrEmpty(primerApellido))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable primerApellido no puede estar vacía";
                fValidacion = false;
            }

            if (string.IsNullOrEmpty(primerNombreM))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable primerNombreM no puede estar vacía";
                fValidacion = false;
            }

            if (string.IsNullOrEmpty(primerApellidoM))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable primerApellidoM no puede estar vacía";
                fValidacion = false;
            }

            if (perFechaNac == DateTime.MinValue)
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable perFechaNac no puede estar vacía";
                fValidacion = false;
            }

            if (string.IsNullOrEmpty(dir_Direccion))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable dir_Direccion no puede estar vacía";
                fValidacion = false;
            }

            if (tia_id == 0)
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable tia_id no puede estar vacía";
                fValidacion = false;
            }

            if (vac_FechaVacuna == DateTime.MinValue)
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable vac_FechaVacuna no puede estar vacía";
                fValidacion = false;
            }

            if (string.IsNullOrEmpty(ins_IdVacuna))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable ins_IdVacuna no puede estar vacía";
                fValidacion = false;
            }

            if (string.IsNullOrEmpty(vac_Lote))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable vac_Lote no puede estar vacía";
                fValidacion = false;
            }

            if (pos_Id == 0)
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable pos_Id no puede estar vacía";
                fValidacion = false;
            }

            if (string.IsNullOrEmpty(per_Func))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable per_Func no puede estar vacía";
                fValidacion = false;
            }

            if (string.IsNullOrEmpty(per_Institucion))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores += " La variable per_Institucion no puede estar vacía";
                fValidacion = false;
            }

            //----------------- Aqui se valida contra las tablas de dominio

            if (!(validarTablasDominio(14, per_TipoId, MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(14, per_TipoIdM, MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(15, per_Institucion, MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(17, cni_id.ToString(), MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(18, etn_idEtnia.ToString(), MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(19, gru_IdGrupo, MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(20, per_Genero, MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(21, perGrupoSanguineo, MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(22, perRh, MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(23, cdm_idCondicion.ToString(), MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(1, ase_id, MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(2, tia_id.ToString(), MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(4, vac_IdNoAplicada.ToString(), MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(5, dos_idNoAplicada.ToString(), MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(35, pes_Peso.ToString(), MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(4, vac_Id.ToString(), MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(5, dos_Id.ToString(), MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(24, pse_Id.ToString(), MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(25, com_Id.ToString(), MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(26, cam_id.ToString(), MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(27, pos_Id.ToString(), MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(32, per_Func.ToString(), MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }

            if (!(validarTablasDominio(15, per_Institucion.ToString(), MensajeValidacion)))
            {
                eResultadoConsulta.resultado = false;
                eResultadoConsulta.errores = MensajeValidacion;
            }


            if (fValidacion)
            {
                //----------------------------------------------------------------------------------------
                // Verificar si el usuario existe en la base de datos
                //----------------------------------------------------------------------------------------

                if (seleccionarPersonaIdContar(per_Id, per_TipoId) > 0 && !string.IsNullOrEmpty(per_Id))
                {
                    eResultadoConsulta.consecutivo = string.Empty;
                    eResultadoConsulta.errores = "La persona ingresada ya existe en el aplicativo, debe seleccionarla en la opción de búsqueda.";
                    eResultadoConsulta.resultado = false;

                    repositorioLog.insertarLog("insertarPersonaVacuna", "La persona ingresada ya existe en el aplicativo, debe seleccionarla en la opción de búsqueda.", null);
                    repositorioLog.insertarLogBD("insertarPersonaVacuna", "La persona ingresada ya existe en el aplicativo, debe seleccionarla en la opción de búsqueda.", null);

                    return eResultadoConsulta;
                }
                else
                {
                    //----------------------------------------------------------------------------------------
                    // Cargar los datos de la persona que fueron digitados en la pantalla al objeto oPersonaE
                    //----------------------------------------------------------------------------------------            
                    ePersona.per_NumeroHijoM = 0;

                    //----------------------------------------------------------
                    // Datos de la Madre        
                    //----------------------------------------------------------
                    ePersona.per_TipoIdM = per_TipoIdM;
                    ePersona.per_IdM = per_IdM;
                    ePersona.per_NumeroHijoM = per_NumeroHijoM;
                    ePersona.primerApellidoM = primerApellidoM.ToUpper();
                    ePersona.segundoApellidoM = segundoApellidoM.ToUpper();
                    ePersona.primerNombreM = primerNombreM.ToUpper();
                    ePersona.segundoNombreM = segundoNombreM.ToUpper();

                    //----------------------------------------------------------
                    // Datos del vacunado
                    //----------------------------------------------------------
                    ePersona.cni_id = cni_id;
                    ePersona.per_TipoId = per_TipoId;
                    ePersona.per_Id = per_Id;
                    if (ePersona.per_TipoId.Equals("CN"))
                    {
                        if (string.IsNullOrEmpty(ePersona.per_Id))
                            ePersona.per_CertNacVivo = "0";
                        else
                            ePersona.per_CertNacVivo = ePersona.per_Id;
                    }
                    else
                    {
                        ePersona.per_CertNacVivo = "0";
                    }
                    ePersona.primerApellido = primerApellido.ToUpper();
                    ePersona.segundoApellido = segundoApellido.ToUpper();
                    ePersona.primerNombre = primerNombre.ToUpper();
                    ePersona.segundoNombre = segundoNombre.ToUpper();
                    ePersona.perFechaNac = perFechaNac;
                    ePersona.per_Genero = per_Genero.ToUpper();
                    ePersona.perGrupoSanguineo = perGrupoSanguineo;
                    ePersona.perRh = perRh;
                    ePersona.etn_idEtnia = etn_idEtnia;
                    ePersona.gru_IdGrupo = gru_IdGrupo;

                    //----------------------------------------------------------
                    // Otros datos del objeto persona
                    //----------------------------------------------------------
                    //ePersona.per_Institucion = per_Institucion
                    ePersona.per_Func = per_Func;
                    ePersona.per_Estado = 2; // Usuario en estado activo 
                    ePersona.per_FechaAlm = DateTime.Now;

                    //----------------------------------------------------------------------------------------
                    // Realizar la validacion de la fecha de nacimiento
                    //----------------------------------------------------------------------------------------
                    if (ePersona.perFechaNac >= fechaBase && Convert.ToDateTime(ePersona.perFechaNac) <= DateTime.Now)
                    {
                        if (per_Id != per_IdM)
                        {
                            //----------------------------------------------------------------------------------------
                            // Se pregunta si la institucion del usuario esta vacia, de ser asi no se puede actualizar
                            //----------------------------------------------------------------------------------------
                            if (!(string.IsNullOrEmpty(ePersona.per_Institucion)))
                            {
                                //----------------------------------------------------------------------------------------
                                // Insertar los datos básicos de una persona
                                //----------------------------------------------------------------------------------------
                                ResultadoConsultaEntity oResultado;
                                oResultado = InsertarPersona(ePersona.per_TipoId,
                                                         ePersona.per_Id,
                                                         Convert.ToInt64(ePersona.per_CertNacVivo),
                                                         ePersona.per_CertDefuncion,
                                                         ePersona.per_TipoIdM,
                                                         ePersona.per_IdM,
                                                         ePersona.per_NumeroHijoM,
                                                         ePersona.primerNombreM,
                                                         ePersona.segundoNombreM,
                                                         ePersona.primerApellidoM,
                                                         ePersona.segundoApellidoM,
                                                         ePersona.primerNombre,
                                                         ePersona.segundoNombre,
                                                         ePersona.primerApellido,
                                                         ePersona.segundoApellido,
                                                         ePersona.perFechaNac,
                                                         ePersona.per_Func,
                                                         ePersona.per_Institucion,
                                                         ePersona.per_Estado,
                                                         ePersona.cni_id,
                                                         ePersona.etn_idEtnia,
                                                         ePersona.gru_IdGrupo,
                                                         ePersona.per_Genero,
                                                         ePersona.perGrupoSanguineo,
                                                         ePersona.perRh);

                                //--------------------------------------------------------------------------
                                // Verificar si los datos de la persona fueron guardados para continuar..
                                //--------------------------------------------------------------------------
                                if (oResultado.resultado && Convert.ToInt64(oResultado.consecutivo) != 0)
                                {
                                    ePersona.per_Consecutivo = Convert.ToInt64(oResultado.consecutivo);
                                    //--------------------------------------------------------------------
                                    // Insertar datos de peso de la persona (Solo recien nacidos)
                                    //--------------------------------------------------------------------
                                    if (!pes_Peso.Equals(string.Empty))
                                    {
                                        ePersona.pes_Peso = pes_Peso;
                                        InsertarPesoPersona(ePersona.per_Consecutivo,
                                                            Convert.ToInt32(ePersona.pes_Peso));
                                    }
                                    //--------------------------------------------------------------------
                                    // Insertar datos de afiliacion
                                    //--------------------------------------------------------------------
                                    PersonaAfiliacionEntity ePersonaAfiliacion = new PersonaAfiliacionEntity();
                                    ePersonaAfiliacion.per_Consecutivo = ePersona.per_Consecutivo;
                                    ePersonaAfiliacion.ase_id = ase_id;
                                    ePersonaAfiliacion.tia_id = tia_id;

                                    //TODO: Validar el origen del regimen al ingreso de una persona
                                    insertarAfiliacionPersona(ePersonaAfiliacion.per_Consecutivo,
                                                              ePersonaAfiliacion.ase_id,
                                                              1,
                                                              ePersonaAfiliacion.tia_id);

                                    //--------------------------------------------------------------------
                                    // Insertar datos Ubicacion               
                                    //--------------------------------------------------------------------
                                    UbicacionPersonaEntity eUbicacion = new UbicacionPersonaEntity();
                                    eUbicacion.per_Consecutivo = ePersona.per_Consecutivo;
                                    eUbicacion.dir_Direccion = dir_Direccion;
                                    eUbicacion.bar_Id = bar_Id;
                                    eUbicacion.Fecha = DateTime.Now;
                                    eUbicacion.Activo = Convert.ToBoolean(1);
                                    eUbicacion.loc_id = loc_id;
                                    eUbicacion.tel_Telefono = tel_Telefono;
                                    eUbicacion.tel_Contacto = tel_Contacto;
                                    eUbicacion.cor_correo = cor_correo;
                                    eUbicacion.dir_mun_id = dir_mun_id;
                                    eUbicacion.dir_dep_Id = dir_dep_Id;
                                    eUbicacion.dir_pais_Id = dir_pais_Id;
                                    eUbicacion.dir_zon_Id = dir_zon_Id;
                                    eUbicacion.upz_Id = upz_Id;

                                    insertarUbicacionPersona(eUbicacion.per_Consecutivo,
                                                            eUbicacion.dir_Direccion,
                                                            eUbicacion.bar_Id,
                                                            eUbicacion.dir_Codigo_direccion,
                                                            Convert.ToString(eUbicacion.upz_Id),
                                                            eUbicacion.dir_CoordenadaX,
                                                            eUbicacion.dir_CoordenadaY,
                                                            eUbicacion.loc_id,
                                                            eUbicacion.dir_Estrato,
                                                            eUbicacion.tel_Telefono,
                                                            eUbicacion.tel_Contacto,
                                                            eUbicacion.cor_correo,
                                                            eUbicacion.dir_mun_id,
                                                            eUbicacion.dir_dep_Id,
                                                            eUbicacion.dir_pais_Id,
                                                            eUbicacion.dir_zon_Id
                                                            );

                                    //--------------------------------------------------------------------
                                    // InsertarCuestionarioPersona
                                    //--------------------------------------------------------------------
                                    //InsertarCuestionario(opersonaE.per_Consecutivo, opersonaE.per_Institucion)

                                    //--------------------------------------------------------------------
                                    // Actualizar la institucion de parto si es necesario
                                    //--------------------------------------------------------------------
                                    ActualizarInstitucionParto(ePersona.per_Consecutivo, per_ParInstitucion);

                                    //--------------------------------------------------------------------
                                    // Actualizar la institucion de parto si es necesario
                                    //--------------------------------------------------------------------
                                    InsertarCausasNoVacunaPersona(cnv_Id, vac_IdNoAplicada, ePersona.per_Consecutivo, dos_idNoAplicada);

                                    //--------------------------------------------------------------------
                                    // Insertar vacuna persona
                                    //--------------------------------------------------------------------
                                    VacunaPersonaEntity eVacunaPersona = new VacunaPersonaEntity();
                                    eVacunaPersona.vac_FechaVacuna = vac_FechaVacuna;
                                    //-------------------------------------------------------------------
                                    // Verificar la fecha de vacunacion
                                    //-------------------------------------------------------------------
                                    if (eVacunaPersona.vac_FechaVacuna >= fechaBase &&
                                       eVacunaPersona.vac_FechaVacuna <= DateTime.Now &&
                                       eVacunaPersona.vac_FechaVacuna >= ePersona.perFechaNac)
                                    {
                                        //------------------------------------------------------------------------------------------------------
                                        // Se valida que todo lo indispensable se haya seleccionado. Vacuna, Presentación y Presentación Vacuna.
                                        //------------------------------------------------------------------------------------------------------
                                        eVacunaPersona.per_Consecutivo = ePersona.per_Consecutivo;
                                        eVacunaPersona.fun_idFunc = per_Func;
                                        eVacunaPersona.cam_id = cam_id;
                                        eVacunaPersona.vac_actualizacion = vac_actualizacion;
                                        eVacunaPersona.cdm_idCondicion = cdm_idCondicion;
                                        eVacunaPersona.vac_Id = vac_Id;
                                        eVacunaPersona.dos_Id = dos_Id;
                                        eVacunaPersona.pse_Id = pse_Id;
                                        eVacunaPersona.ins_Id = ins_IdVacuna;
                                        eVacunaPersona.com_Id = com_Id;
                                        eVacunaPersona.pos_Id = pos_Id;
                                        eVacunaPersona.vac_Lote = vac_Lote;
                                        //-------------------------------------------------------------------
                                        // Consultar la institucion a la cual pertenece el usuario del sistema.
                                        //-------------------------------------------------------------------                                          
                                        eVacunaPersona.ins_Id = per_Institucion;

                                        //----------------------------------------------------------------
                                        // Calculo de la edad al momento de la vacuna
                                        //----------------------------------------------------------------  

                                        calculoEdad(ePersona.perFechaNac, eVacunaPersona.vac_FechaVacuna, eVacunaPersona.vac_EdadVacunaAnios, eVacunaPersona.vac_EdadVacunaMeses, eVacunaPersona.vac_EdadVacunaDias);
                                        eVacunaPersona.vac_EdadVacunaTotalDias = (eVacunaPersona.vac_EdadVacunaAnios * 12 * 30) + (eVacunaPersona.vac_EdadVacunaMeses * 30) + eVacunaPersona.vac_EdadVacunaDias;
                                        //----------------------------------------------------------------
                                        // Guardar vacuna
                                        //----------------------------------------------------------------                                  
                                        repositorioVacunas.InsertarVacunaPersona(eVacunaPersona.per_Consecutivo,
                                                                eVacunaPersona.vac_Id,
                                                                eVacunaPersona.dos_Id,
                                                                eVacunaPersona.pse_Id,
                                                                eVacunaPersona.cam_id,
                                                                eVacunaPersona.vac_FechaVacuna,
                                                                eVacunaPersona.vac_actualizacion,
                                                                eVacunaPersona.ins_Id,
                                                                eVacunaPersona.fun_idFunc,
                                                                eVacunaPersona.com_Id,
                                                                eVacunaPersona.pos_Id,
                                                                eVacunaPersona.vac_Lote,
                                                                eVacunaPersona.vac_EdadVacunaAnios,
                                                                eVacunaPersona.vac_EdadVacunaMeses,
                                                                eVacunaPersona.vac_EdadVacunaDias,
                                                                eVacunaPersona.vac_EdadVacunaTotalDias,
                                                                eVacunaPersona.cdm_idCondicion);

                                        //----------------------------------------------------------------
                                        //Actualizar el esquema de vacunacion (Seguimiento)
                                        //----------------------------------------------------------------                                
                                        ActualizarEsquemaSeguimientoPersona(ePersona.per_Consecutivo);

                                        //--------------------------------------------------------------------
                                        // Mostrar mensajes de guardado correcto               
                                        //--------------------------------------------------------------------                            
                                        eResultadoConsulta.consecutivo = string.Empty;
                                        eResultadoConsulta.errores = "El registro se ha almacenado correctamente.";
                                        eResultadoConsulta.resultado = true;
                                        return eResultadoConsulta;
                                    }
                                    else
                                    {
                                        eResultadoConsulta.consecutivo = string.Empty;
                                        eResultadoConsulta.errores = "La fecha de vacunación es inconsistente, verifiquela.";
                                        eResultadoConsulta.resultado = false;

                                        repositorioLog.insertarLog("insertarPersonaVacuna", "La fecha de vacunación es inconsistente, verifiquela.", null);
                                        repositorioLog.insertarLogBD("insertarPersonaVacuna", "La fecha de vacunación es inconsistente, verifiquela.", null);

                                        return eResultadoConsulta;
                                    }
                                }
                                else
                                {
                                    eResultadoConsulta.consecutivo = string.Empty;
                                    eResultadoConsulta.errores = "El registro NO se ha almacenado. Intente nuevamente más tarde.";
                                    eResultadoConsulta.resultado = false;

                                    repositorioLog.insertarLog("insertarPersonaVacuna", "El registro NO se ha almacenado. Intente nuevamente más tarde", null);
                                    repositorioLog.insertarLogBD("insertarPersonaVacuna", "El registro NO se ha almacenado. Intente nuevamente más tarde", null);

                                    return eResultadoConsulta;
                                }
                            }
                            else
                            {
                                eResultadoConsulta.consecutivo = string.Empty;
                                eResultadoConsulta.errores = "No hay institución seleccionada, deberá volver a conectarse.";
                                eResultadoConsulta.resultado = false;

                                repositorioLog.insertarLog("insertarPersonaVacuna", "No hay institución seleccionada, deberá volver a conectarse", null);
                                repositorioLog.insertarLogBD("insertarPersonaVacuna", "No hay institución seleccionada, deberá volver a conectarse", null);

                                return eResultadoConsulta;

                            }
                        }
                        else
                        {
                            eResultadoConsulta.consecutivo = string.Empty;
                            eResultadoConsulta.errores = "El Numero de Identificacion del Menor es igual al de la Madre.";
                            eResultadoConsulta.resultado = false;

                            repositorioLog.insertarLog("insertarPersonaVacuna", "El Numero de Identificacion del Menor es igual al de la Madre.", null);
                            repositorioLog.insertarLogBD("insertarPersonaVacuna", "El Numero de Identificacion del Menor es igual al de la Madre.", null);

                            return eResultadoConsulta;
                        }
                    }
                    else
                    {
                        eResultadoConsulta.consecutivo = string.Empty;
                        eResultadoConsulta.errores = "La fecha de nacimiento es inconsistente, verifiquela.";
                        eResultadoConsulta.resultado = false;

                        repositorioLog.insertarLog("insertarPersonaVacuna", "La fecha de nacimiento es inconsistente, verifiquela", null);
                        repositorioLog.insertarLogBD("insertarPersonaVacuna", "La fecha de nacimiento es inconsistente, verifiquela", null);

                        return eResultadoConsulta;
                    }
                }
            }
            return eResultadoConsulta;
        }

        public ResultadoConsultaEntity InsertarPesoPersona(long per_Consecutivo, int pes_Peso)
        {
            ResultadoConsultaEntity oResultado = new ResultadoConsultaEntity();

            if (per_Consecutivo != 0 && pes_Peso != 0)
            {
                SqlParameter[] arParms = new SqlParameter[1];

                arParms[0] = new SqlParameter("@per_Consecutivo", 0);
                arParms[0].Value = per_Consecutivo;

                arParms[1] = new SqlParameter("@pes_Peso", 8);
                arParms[1].Value = pes_Peso;

                try
                {
                    if (SqlHelper.ExecuteNonQuery(cadena, "pa_InsertarPesoWS", arParms) > 0)
                    {
                        oResultado.resultado = true;
                        oResultado.errores = string.Empty;

                        repositorioLog.insertarLog("insertarPesoPersona()", "CORRECTO", arParms);
                        repositorioLog.insertarLogBD("insertarPesoPersona()", "CORRECTO", arParms);
                    }
                    else
                    {
                        oResultado.resultado = false;
                        oResultado.errores = "Registro no identificado.";

                        repositorioLog.insertarLog("insertarPesoPersona()", "Registro no ingresado", arParms);
                        repositorioLog.insertarLogBD("insertarPesoPersona()", "Registro no ingresado", arParms);
                    }
                }
                catch (SqlException e)
                {
                    string errorMessage = e.Message;
                    int errorCode = e.ErrorCode;

                    oResultado.resultado = false;
                    oResultado.errores = errorMessage;

                    repositorioLog.insertarLog("insertarPesoPersona()", e.Message, arParms);
                    repositorioLog.insertarLogBD("insertarPesoPersona()", e.Message, arParms);
                }
            }
            else
            {
                oResultado.resultado = false;
                oResultado.errores = "El per_consecutivo fue envido como 0";
            }
            return oResultado;
        }

        public ResultadoConsultaEntity ActualizarInstitucionParto(long per_Consecutivo, string per_ParInstitucion)
        {
            ResultadoConsultaEntity oResultado = new ResultadoConsultaEntity();

            if (per_Consecutivo != 0 && per_ParInstitucion != "")
            {
                SqlParameter[] arParms = new SqlParameter[1];

                arParms[0] = new SqlParameter("@per_Consecutivo", 0);
                arParms[0].Value = per_Consecutivo;

                arParms[1] = new SqlParameter("@per_ParInstitucion", 3);
                arParms[1].Value = per_ParInstitucion;

                try
                {
                    if (SqlHelper.ExecuteNonQuery(cadena, "pa_ActualizarInstitucionPartoWS", arParms) > 0)
                    {
                        oResultado.resultado = true;
                        oResultado.errores = string.Empty;

                        repositorioLog.insertarLog("ActualizarInstitucionParto()", "CORRECTO", arParms);
                        repositorioLog.insertarLogBD("ActualizarInstitucionParto()", "CORRECTO", arParms);
                    }
                    else
                    {
                        oResultado.resultado = false;
                        oResultado.errores = "Registro no ingresado.";

                        repositorioLog.insertarLog("ActualizarInstitucionParto()", "Registro no ingresado", arParms);
                        repositorioLog.insertarLogBD("ActualizarInstitucionParto()", "Registro no ingresado", arParms);
                    }
                }
                catch (SqlException e)
                {
                    string errorMessage = e.Message;
                    int errorCode = e.ErrorCode;

                    oResultado.resultado = false;
                    oResultado.errores = errorMessage;

                    repositorioLog.insertarLog("ActualizarInstitucionParto()", e.Message, arParms);
                    repositorioLog.insertarLogBD("ActualizarInstitucionParto()", e.Message, arParms);
                }
            }
            else
            {
                oResultado.resultado = false;
                oResultado.errores = "El per_consecutivo fue envido como 0";
            }
            return oResultado;
        }

        public ResultadoConsultaEntity InsertarCausasNoVacunaPersona(int cnv_Id, int vac_Id, long per_Consecutivo, long dos_id)
        {
            ResultadoConsultaEntity oResultado = new ResultadoConsultaEntity();

            if (per_Consecutivo != 0)
            {
                SqlParameter[] arParms = new SqlParameter[3];

                arParms[0] = new SqlParameter("@cnv_Id", 8);
                arParms[0].Value = cnv_Id;

                arParms[1] = new SqlParameter("@vac_Id", 8);
                arParms[1].Value = vac_Id;

                arParms[2] = new SqlParameter("@per_Consecutivo", 0);
                arParms[2].Value = per_Consecutivo;

                arParms[3] = new SqlParameter("@dos_id", 8);
                arParms[3].Value = dos_id;

                try
                {
                    if (SqlHelper.ExecuteNonQuery(cadena, "pa_InsertarCausasNoVacunacionWS", arParms) > 0)
                    {
                        oResultado.resultado = true;
                        oResultado.errores = string.Empty;

                        repositorioLog.insertarLog("InsertarCausasNoVacunaPersona()", "CORRECTO", arParms);
                    }
                    else
                    {
                        oResultado.resultado = false;
                        oResultado.errores = "Registro no realizado.";

                        repositorioLog.insertarLog("InsertarCausasNoVacunaPersona()", "Registro no ingresado", arParms);
                    }
                }
                catch (SqlException e)
                {
                    string errorMessage = e.Message;
                    int errorCode = e.ErrorCode;

                    oResultado.resultado = false;
                    oResultado.errores = errorMessage;

                    repositorioLog.insertarLog("InsertarCausasNoVacunaPersona()", e.Message, arParms);
                }
            }
            else
            {
                oResultado.resultado = false;
                oResultado.errores = "El per_consecutivo fue envido como 0";
            }
            return oResultado;
        }

        public ResultadoConsultaEntity ActualizarEsquemaSeguimientoPersona(long per_Consecutivo)
        {
            ResultadoConsultaEntity oResultado = new ResultadoConsultaEntity();

            if (per_Consecutivo != 0)
            {
                SqlParameter[] arParms = new SqlParameter[3];

                arParms[0] = new SqlParameter("@per_Consecutivo", 0);
                arParms[0].Value = per_Consecutivo;

                try
                {
                    if (Convert.ToInt64(SqlHelper.ExecuteNonQuery(cadena, "pa_InsertarCausasNoVacunacionWS", arParms)) > 0)
                    {
                        oResultado.resultado = true;
                        oResultado.errores = string.Empty;

                        repositorioLog.insertarLog("ActualizarEsquemaSegumientoPersona()", "CORRECTO", arParms);
                        repositorioLog.insertarLogBD("ActualizarEsquemaSegumientoPersona()", "CORRECTO", arParms);
                    }
                    else
                    {
                        oResultado.resultado = false;
                        oResultado.errores = "Registro no realizado.";

                        repositorioLog.insertarLog("ActualizarEsquemaSegumientoPersona()", "Registro no ingresado", arParms);
                        repositorioLog.insertarLogBD("ActualizarEsquemaSegumientoPersona()", "Registro no ingresado", arParms);
                    }
                }
                catch (SqlException e)
                {
                    string errorMessage = e.Message;
                    int errorCode = e.ErrorCode;

                    oResultado.resultado = false;
                    oResultado.errores = errorMessage;

                    repositorioLog.insertarLog("ActualizarEsquemaSegumientoPersona()", e.Message, arParms);
                    repositorioLog.insertarLogBD("ActualizarEsquemaSegumientoPersona()", e.Message, arParms);
                }
            }
            else
            {
                oResultado.resultado = false;
                oResultado.errores = "El per_consecutivo fue envido como 0";
            }
            return oResultado;
        }

        public IEnumerable<PersonaEntity> seleccionarPersonaBusqueda(string TipoIdVacunado, string NumeroIdVacunado, string PrimerNombreVacunado, string SegundoNombreVacunado, string PrimerApellidoVacunado, string SegundoApellidoVacunado, string per_parInstitucion, DateTime per_FechaNac, string TipoIdentificacionMadre, string NumeroIdentificacionMadre, string PrimerNombreMadre, string SegundoNombreMadre, string PrimerApellidoMadre, string SegundoApellidoMadre, int grupoEtareo)
        {
            SqlParameter[] arParms = new SqlParameter[1];
            arParms[0] = new SqlParameter("@NumeroIdVacunado", 3);
            arParms[0].Value = NumeroIdVacunado;

            repositorioLog.insertarLogBD("seleccionarPersonaBusqueda()", "CORRECTO", arParms);


            return (IEnumerable<PersonaEntity>)seleccionarPersonaBusquedaAttr(TipoIdVacunado, NumeroIdVacunado,
                                                PrimerNombreVacunado, SegundoNombreVacunado, PrimerApellidoVacunado, SegundoApellidoVacunado,
                                                per_parInstitucion, per_FechaNac, TipoIdentificacionMadre, NumeroIdentificacionMadre,
                                                PrimerNombreMadre, SegundoNombreMadre, PrimerApellidoMadre, SegundoApellidoMadre,
                                                grupoEtareo, true, false);
        }

        

        public Boolean validarTablasDominio(short id_Tabla, string codigoTabla, string MensajeValidacion)
        {
            int nroRegistros = 0;
            Boolean fValidacion = true;

            SqlParameter[] arParms = new SqlParameter[2];
            arParms[0] = new SqlParameter("@id_Tabla", 8);
            arParms[0].Value = id_Tabla;
            arParms[1] = new SqlParameter("@codigoTabla", 22);
            arParms[1].Value = codigoTabla;

            nroRegistros = Convert.ToInt32(SqlHelper.ExecuteScalar(cadena, "pa_ValidarTablasDominio", arParms));
            if (nroRegistros <= 0)
            {
                fValidacion = false;
                MensajeValidacion = "El código: " + codigoTabla + " no existe en la tabla: " + nombreTabla(id_Tabla) + " ";
            }
            return fValidacion;
        }

        public UbicacionPersonaEntity seleccionarUbicacionPersona(long per_Consecutivo)
        {
            SqlParameter[] arParms = new SqlParameter[1];
            arParms[0] = new SqlParameter("@per_Consecutivo", 0);
            arParms[0].Value = per_Consecutivo;
            UbicacionPersonaEntity oPersonaUbicacion = new UbicacionPersonaEntity();
            DataSet dsPersonaUbicacion = SqlHelper.ExecuteDataset(cadena, "pa_SeleccionarUbicacionPersona", arParms);
            if (dsPersonaUbicacion != null && dsPersonaUbicacion.Tables[0].Rows.Count > 0)
            {
                var row = dsPersonaUbicacion.Tables[0].Rows[0];
                oPersonaUbicacion.per_Consecutivo = Convert.ToInt64(row["per_Consecutivo"].ToString());
                oPersonaUbicacion.dir_Direccion = row["dir_Direccion"].ToString();
                oPersonaUbicacion.bar_Id = row["bar_Id"].ToString();
                oPersonaUbicacion.bar_Nombre = row["bar_Nombre"].ToString();
                if (!string.IsNullOrEmpty(row["upz_Id"].ToString()))
                {
                    oPersonaUbicacion.upz_Id = Convert.ToInt32(row["upz_Id"].ToString());
                }
                oPersonaUbicacion.upz_Nombre = row["upz_Nombre"].ToString();
                if (!string.IsNullOrEmpty(row["loc_Id"].ToString()))
                {
                    oPersonaUbicacion.loc_id = Convert.ToInt32(row["loc_Id"].ToString());
                }
                oPersonaUbicacion.loc_Nombre = row["loc_Nombre"].ToString();
                oPersonaUbicacion.dir_Codigo_direccion = row["dir_Codigo_direccion"].ToString();
                oPersonaUbicacion.dir_CoordenadaX = row["dir_CoordenadaX"].ToString();
                oPersonaUbicacion.dir_CoordenadaY = row["dir_CoordenadaY"].ToString();
                oPersonaUbicacion.tel_Contacto = row["tel_Contacto"].ToString();
                oPersonaUbicacion.tel_Telefono = row["tel_Telefono"].ToString();
                oPersonaUbicacion.cor_correo = row["cor_correo"].ToString();
                if (!string.IsNullOrEmpty(row["dir_mun_id"].ToString()))
                {
                    oPersonaUbicacion.dir_mun_id = Convert.ToInt32(row["dir_mun_id"].ToString());
                }
                if (!string.IsNullOrEmpty(row["dir_dep_Id"].ToString()))
                {
                    oPersonaUbicacion.dir_dep_Id = Convert.ToInt32(row["dir_dep_Id"].ToString());
                }
                oPersonaUbicacion.dir_pais_Id = row["dir_pais_Id"].ToString();
                if (!string.IsNullOrEmpty(row["dir_zon_Id"].ToString()))
                {
                    oPersonaUbicacion.dir_zon_Id = Convert.ToInt32(row["dir_zon_Id"].ToString());
                }

                repositorioLog.insertarLogBD("seleccionarPersonaBusqueda()", "CORRECTO", arParms);
                repositorioLog.insertarLog("seleccionarUbicacionPersona()", "CORRECTO", arParms);
            }

            return oPersonaUbicacion;
        }

        public PersonaAfiliacionEntity seleccionarAfiliacionPersona(long per_Consecutivo)
        {
            PersonaAfiliacionEntity oPersonaAfiliacion = new PersonaAfiliacionEntity();
            if (per_Consecutivo != 0)
            {
                SqlParameter[] arParms = new SqlParameter[1];
                arParms[0] = new SqlParameter("@per_Consecutivo", 0);
                arParms[0].Value = per_Consecutivo;

                DataSet dsPersonaAfiliacion = SqlHelper.ExecuteDataset(cadena, "pa_SeleccionarAfiliacionPersona", arParms);
                if (dsPersonaAfiliacion != null && dsPersonaAfiliacion.Tables[0].Rows.Count > 0)
                {
                    oPersonaAfiliacion.per_Consecutivo = Convert.ToInt64(dsPersonaAfiliacion.Tables[0].Rows[0]["per_Consecutivo"].ToString());
                    oPersonaAfiliacion.ase_id = dsPersonaAfiliacion.Tables[0].Rows[0]["ase_id"].ToString();
                    oPersonaAfiliacion.ase_nombre = dsPersonaAfiliacion.Tables[0].Rows[0]["ase_nombre"].ToString();
                    oPersonaAfiliacion.reg_id = Convert.ToInt32(dsPersonaAfiliacion.Tables[0].Rows[0]["reg_id"].ToString());
                    oPersonaAfiliacion.reg_Nombre = dsPersonaAfiliacion.Tables[0].Rows[0]["reg_Nombre"].ToString();
                    if (!string.IsNullOrEmpty(dsPersonaAfiliacion.Tables[0].Rows[0]["tia_id"].ToString()))
                    {
                        oPersonaAfiliacion.tia_id = Convert.ToInt32(dsPersonaAfiliacion.Tables[0].Rows[0]["tia_id"].ToString());
                    }
                }
                repositorioLog.insertarLogBD("seleccionarVacunasPersona()", "CORRECTO", arParms);
                repositorioLog.insertarLog("seleccionarAfiliacionPersona()", "CORRECTO", arParms);
            }
            return oPersonaAfiliacion;
        }

        public string seleccionarEstadoContactenos(long id_Caso)
        {
            TablaDominioEntity cTablaDominio = new TablaDominioEntity();

            SqlParameter[] arParms = new SqlParameter[1];
            arParms[0] = new SqlParameter("@idCaso", 0);
            arParms[0].Value = id_Caso;

            string estado = Convert.ToString(SqlHelper.ExecuteScalar(cadena, "pa_SeleccionarContactenosEstadoPorId", arParms));

            repositorioLog.insertarLogBD("seleccionarEstadoContactenos()", "CORRECTO", arParms);

            return estado;
        }

        public List<TablaDominioEntity> seleccionarTablasConNovedades()
        {
            List<TablaDominioEntity> cTablaDominios = new List<TablaDominioEntity>();
            DataSet dsTablasDominio = SqlHelper.ExecuteDataset(cadena, "pa_SeleccionarTablasDominioModificadas");
            if (dsTablasDominio != null)
            {
                foreach (DataRow dr in dsTablasDominio.Tables[0].Rows)
                {
                    TablaDominioEntity eTablaDominio = new TablaDominioEntity();
                    eTablaDominio.td_id = dr[0].ToString();
                    eTablaDominio.td_descripcion = dr[1].ToString();
                    cTablaDominios.Add(eTablaDominio);
                }
            }
            return cTablaDominios;
        }

        public List<TablaDominioEntity> seleccionarTablaDominio(short id_Tabla)
        {
            List<TablaDominioEntity> cTablaDominio = new List<TablaDominioEntity>();

            SqlParameter[] arParms = new SqlParameter[1];
            arParms[0] = new SqlParameter("@id_Tabla", 16);
            arParms[0].Value = id_Tabla;

            DataSet dsTablaDominio = SqlHelper.ExecuteDataset(cadena, "pa_SeleccionarTablasDominio", arParms);

            if (dsTablaDominio != null && dsTablaDominio.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsTablaDominio.Tables[0].Rows)
                {
                    TablaDominioEntity eTablaDominio = new TablaDominioEntity();
                    eTablaDominio.td_id = dr[0].ToString();
                    eTablaDominio.td_descripcion = dr[1].ToString();
                    cTablaDominio.Add(eTablaDominio);
                }
            }

            return cTablaDominio;
        }

        public ResultadoConsultaEntity actualizarPersona(ActualizarPersonaRequestDTO requestDTO)
        {
            ResultadoConsultaEntity oResultado = new ResultadoConsultaEntity();
            Boolean fValidacion = true;
            string MensajeValidacion = string.Empty;
            string MsgValidacion = "";

            if (requestDTO.per_consecutivo == 0)
            {
                oResultado.resultado = false;
                oResultado.errores = "la variable per_consecutivo no puede ser 0";
                fValidacion = false;
            }
            if (string.IsNullOrEmpty(requestDTO.per_TipoId))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_tipoId no puede estar vacía";
                fValidacion = false;
            }
            if (string.IsNullOrEmpty(requestDTO.per_Id))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_Id no puede estar vacía";
                fValidacion = false;
            }
            if (string.IsNullOrEmpty(requestDTO.per_TipoIdM))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_TipoIdM no puede estar vacía";
                fValidacion = false;
            }
            if (string.IsNullOrEmpty(requestDTO.per_IdM))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_IdM no puede estar vacía";
                fValidacion = false;
            }
            if (requestDTO.per_NumeroHijoM == 0)
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_NumeroHijoM no puede ser 0";
                fValidacion = false;
            }
            if (string.IsNullOrEmpty(requestDTO.per_Nombre1M))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_Nombre1M no puede estar vacía";
                fValidacion = false;
            }
            if (string.IsNullOrEmpty(requestDTO.per_Apellido1M))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_Apellido1M no puede estar vacía";
                fValidacion = false;
            }
            if (string.IsNullOrEmpty(requestDTO.per_Nombre1))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_Nombre1 no puede estar vacía";
                fValidacion = false;
            }
            if (string.IsNullOrEmpty(requestDTO.per_Apellido1))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_Apellido1 no puede estar vacía";
                fValidacion = false;
            }
            if (requestDTO.per_FechaNac == DateTime.MinValue)
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_FechaNac no puede estar vacía";
                fValidacion = false;
            }
            if (string.IsNullOrEmpty(requestDTO.per_Func))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_FechaNac no puede estar vacía";
                fValidacion = false;
            }

            if (string.IsNullOrEmpty(requestDTO.per_Institucion))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_Institucion no puede estar vacía";
                fValidacion = false;
            }
            if (requestDTO.per_idEtnia == 0)
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_idEtnia no puede ser 0";
                fValidacion = false;
            }
            if (string.IsNullOrEmpty(requestDTO.per_IdGenero))
            {
                oResultado.resultado = false;
                oResultado.errores += " La variable per_Id ";
                fValidacion = false;

            }


            if (!validarTablasDominio(36, requestDTO.per_consecutivo.ToString(), MensajeValidacion))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }
            if (!validarTablasDominio(14, requestDTO.per_TipoId, MensajeValidacion))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }
            if (!validarTablasDominio(14, requestDTO.per_TipoIdM, MensajeValidacion))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }
            if (!validarTablasDominio(32, requestDTO.per_Func, MensajeValidacion))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }
            if (!validarTablasDominio(15, requestDTO.per_Institucion, MensajeValidacion))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }
            if (!validarTablasDominio(16, requestDTO.per_Estado.ToString(), MensajeValidacion))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }
            if (!validarTablasDominio(17, requestDTO.per_cni_id.ToString(), MensajeValidacion))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }
            if (!validarTablasDominio(18, requestDTO.per_idEtnia.ToString(), MensajeValidacion))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }
            if (!validarTablasDominio(20, requestDTO.per_IdGenero.ToString(), MensajeValidacion))
            {
                oResultado.resultado = false;
                oResultado.errores += MensajeValidacion;
            }
            if (fValidacion)
            {
                SqlParameter[] arParms = new SqlParameter[25];

                arParms[0] = new SqlParameter("@per_Consecutivo", 0);
                arParms[0].Value = requestDTO.per_consecutivo;

                arParms[1] = new SqlParameter("@per_TipoId", 3);
                if (string.IsNullOrEmpty(requestDTO.per_TipoId))
                {
                    arParms[1].Value = DBNull.Value;
                }
                else
                {
                    arParms[1].Value = requestDTO.per_TipoId;
                }
                arParms[2] = new SqlParameter("@per_Id", 22);

                if (string.IsNullOrEmpty(requestDTO.per_Id))
                {
                    arParms[2].Value = DBNull.Value;
                }
                else
                {
                    arParms[2].Value = requestDTO.per_Id;
                }
                arParms[3] = new SqlParameter("@per_CertNacVivo", 0);
                if (string.IsNullOrEmpty(requestDTO.per_CertNacVivo.ToString()))
                {
                    arParms[3].Value = DBNull.Value;
                }
                else
                {
                    arParms[3].Value = requestDTO.per_CertNacVivo;
                }
                arParms[4] = new SqlParameter("@per_CertDefuncion", 22);
                if (string.IsNullOrEmpty(requestDTO.per_CertDefuncion.ToString()))
                {
                    arParms[4].Value = DBNull.Value;
                }
                else
                {
                    arParms[4].Value = requestDTO.per_CertDefuncion;
                }
                arParms[5] = new SqlParameter("@per_TipoIdM", 3);
                if (string.IsNullOrEmpty(requestDTO.per_TipoIdM))
                {
                    arParms[5].Value = DBNull.Value;
                }
                else
                {
                    arParms[5].Value = requestDTO.per_TipoIdM;
                }
                arParms[6] = new SqlParameter("@per_IdM", 22);
                if (string.IsNullOrEmpty(requestDTO.per_IdM))
                {
                    arParms[6].Value = DBNull.Value;
                }
                else
                {
                    arParms[6].Value = requestDTO.per_IdM;
                }
                arParms[7] = new SqlParameter("@per_NumeroHijoM", 16);
                if (string.IsNullOrEmpty(requestDTO.per_NumeroHijoM.ToString()))
                {
                    arParms[7].Value = DBNull.Value;
                }
                else
                {
                    arParms[7].Value = requestDTO.per_NumeroHijoM.ToString();
                }
                arParms[8] = new SqlParameter("@per_Nombre1M", 22);
                if (string.IsNullOrEmpty(requestDTO.per_Nombre1M))
                {
                    arParms[8].Value = DBNull.Value;
                }
                else
                {
                    arParms[8].Value = requestDTO.per_Nombre1M;
                }
                arParms[9] = new SqlParameter("@per_Nombre2M", 22);
                if (string.IsNullOrEmpty(requestDTO.per_Nombre2M))
                {
                    arParms[9].Value = DBNull.Value;
                }
                else
                {
                    arParms[9].Value = requestDTO.per_Nombre2M;
                }
                arParms[10] = new SqlParameter("@per_Apellido1M", 22);
                if (string.IsNullOrEmpty(requestDTO.per_Apellido1M))
                {
                    arParms[10].Value = DBNull.Value;
                }
                else
                {
                    arParms[10].Value = requestDTO.per_Apellido1M;
                }
                arParms[11] = new SqlParameter("@per_Apellido2M", 22);
                if (string.IsNullOrEmpty(requestDTO.per_Apellido2M))
                {
                    arParms[11].Value = DBNull.Value;
                }
                else
                {
                    arParms[11].Value = requestDTO.per_Apellido2M;
                }
                arParms[12] = new SqlParameter("@per_Nombre1", 22);
                if (string.IsNullOrEmpty(requestDTO.per_Nombre1))
                {
                    arParms[12].Value = DBNull.Value;
                }
                else
                {
                    arParms[12].Value = requestDTO.per_Nombre1;
                }

                arParms[13] = new SqlParameter("@per_Nombre2", 22);
                if (string.IsNullOrEmpty(requestDTO.per_Nombre2))
                {
                    arParms[13].Value = DBNull.Value;
                }
                else
                {
                    arParms[13].Value = requestDTO.per_Nombre2;
                }
                arParms[14] = new SqlParameter("@per_Apellido1", 22);
                if (string.IsNullOrEmpty(requestDTO.per_Apellido1))
                {
                    arParms[14].Value = DBNull.Value;
                }
                else
                {
                    arParms[14].Value = requestDTO.per_Apellido1;
                }
                arParms[15] = new SqlParameter("@per_Apellido2", 22);
                if (string.IsNullOrEmpty(requestDTO.per_Apellido2))
                {
                    arParms[15].Value = DBNull.Value;
                }
                else
                {
                    arParms[15].Value = requestDTO.per_Apellido2;
                }
                arParms[16] = new SqlParameter("@per_FechaNac", 31);
                if (requestDTO.per_FechaNac == DateTime.MinValue)
                {
                    arParms[16].Value = DBNull.Value;
                }
                else
                {
                    arParms[16].Value = requestDTO.per_FechaNac;
                }
                arParms[17] = new SqlParameter("@per_Func", 12);
                if (string.IsNullOrEmpty(requestDTO.per_Func))
                {
                    arParms[17].Value = DBNull.Value;
                }
                else
                {
                    arParms[17].Value = requestDTO.per_Func;
                }
                arParms[18] = new SqlParameter("@per_Institucion", 3);
                if (string.IsNullOrEmpty(requestDTO.per_Institucion))
                {
                    arParms[18].Value = DBNull.Value;
                }
                else
                {
                    arParms[18].Value = requestDTO.per_Institucion;
                }
                arParms[19] = new SqlParameter("@per_Estado", 8);
                if (requestDTO.per_Estado == 0)
                {
                    arParms[19].Value = DBNull.Value;
                }
                else
                {
                    arParms[19].Value = requestDTO.per_Estado;
                }
                arParms[20] = new SqlParameter("@per_cni_id", 8);
                if (requestDTO.per_cni_id == 0)
                {
                    arParms[20].Value = DBNull.Value;
                }
                else
                {
                    arParms[20].Value = requestDTO.per_cni_id;
                }
                arParms[21] = new SqlParameter("@per_idEtnia", 8);
                if (requestDTO.per_idEtnia == 0)
                {
                    arParms[21].Value = DBNull.Value;
                }
                else
                {
                    arParms[21].Value = requestDTO.per_idEtnia;
                }
                arParms[22] = new SqlParameter("@per_IdGrupoPoblacional", 3);
                if (requestDTO.per_IdGrupoPoblacional == "0")
                {
                    arParms[22].Value = DBNull.Value;
                }
                else
                {
                    arParms[22].Value = requestDTO.per_IdGrupoPoblacional;
                }
                arParms[23] = new SqlParameter("@per_IdGenero", 3);
                if (string.IsNullOrEmpty(requestDTO.per_IdGenero))
                {
                    arParms[23].Value = DBNull.Value;
                }
                else
                {
                    arParms[23].Value = requestDTO.per_IdGenero;
                }
                arParms[24] = new SqlParameter("@per_IdGrupoSanguineo", 3);
                if (string.IsNullOrEmpty(requestDTO.per_IdGrupoSanguineo))
                {
                    arParms[24].Value = DBNull.Value;
                }
                else
                {
                    arParms[24].Value = requestDTO.per_IdGrupoSanguineo;
                }
                arParms[25] = new SqlParameter("@per_IdRh", 3);
                if (string.IsNullOrEmpty(requestDTO.per_IdRh))
                {
                    arParms[25].Value = DBNull.Value;
                }
                else
                {
                    arParms[25].Value = requestDTO.per_IdRh;
                }

                try
                {
                    if (arParms[1].Value.ToString().Equals("RC") || arParms[1].Value.ToString().Equals("TI"))
                    {
                        repositorioLog.insertarLog("actualizarPersona()", "VALIDACION(RC-TI)", arParms);
                        repositorioLog.insertarLogBD("actualizarPersona()", "VALIDACION(RC-TI)", arParms);
                    }

                    if (SqlHelper.ExecuteNonQuery(cadena, "pa_ActualizarPersonaWS", arParms) > 0)
                    {
                        oResultado.resultado = true;
                        oResultado.errores = string.Empty;
                        oResultado.consecutivo = requestDTO.per_consecutivo.ToString();

                        repositorioLog.insertarLog("actualizarPersona()", "CORRECTO", arParms);
                        repositorioLog.insertarLogBD("actualizarPersona()", "CORRECTO", arParms);
                    }
                    else
                    {
                        oResultado.resultado = false;
                        oResultado.errores = "Registro no identificado.";

                        repositorioLog.insertarLog("actualizarPersona()", "No actualizo ningun registro", arParms);
                        repositorioLog.insertarLogBD("actualizarPersona()", "No actualizo ningun registro", arParms);
                    }
                }
                catch (SqlException ex)
                {
                    string errorMessage = ex.Message;
                    int errorCode = ex.ErrorCode;

                    oResultado.resultado = false;
                    oResultado.errores = errorMessage;

                    repositorioLog.insertarLog("actualizarPersona()", ex.Message, arParms);
                    repositorioLog.insertarLogBD("actualizarPersona()", ex.Message, arParms);

                }
            }
            else
            {
                oResultado.resultado = false;
                oResultado.errores = MsgValidacion;
            }
            return oResultado;
        }
    }
}

