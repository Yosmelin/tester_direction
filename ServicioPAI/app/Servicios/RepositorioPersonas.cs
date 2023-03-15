using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using ServicioPAI.Entidades;
using ServicioPAI.Helper;
using ServicioPAI.Modelos;
using System.Collections;
using System.Data;
using System.Runtime.InteropServices;

namespace ServicioPAI.Servicios
{
    public interface IRepositorioPersonas
    {       
        List<PersonaEntity> seleccionarPersonaBusquedaAttr(string TipoIdVacunado, string NumeroIdVacunado, string PrimerNombreVacunado, string SegundoNombreVacunado, string PrimerApellidoVacunado, string SegundoApellidoVacunado, string per_parInstitucion, DateTime per_FechaNac, string TipoIdentificacionMadre, string NumeroIdentificacionMadre, string PrimerNombreMadre, string SegundoNombreMadre, string PrimerApellidoMadre, string SegundoApellidoMadre, int grupoEtareo, bool bGetMadre, bool bGetHdocum);
        string seleccionarPersonaIdentificacion(long per_Consecutivo);
    }
    public class RepositorioPersonas : IRepositorioPersonas
    {
        private readonly string cadena;       
        private readonly IRepositorioLog repositorioLog;

        public RepositorioPersonas(IConfiguration configuration,           
            IRepositorioLog repositorioLog)
        {
            this.cadena = configuration.GetConnectionString("DefaultConnection");            
            this.repositorioLog = repositorioLog;
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
            {
                arParms[0].Value = DBNull.Value;
            }
            else
            {
                arParms[0].Value = TipoIdVacunado;
            }

            arParms[1] = new SqlParameter("@NumeroIdVacunado", 22);
            if (string.IsNullOrEmpty(NumeroIdVacunado))
            {
                arParms[1].Value = DBNull.Value;
            }
            else
            {
                arParms[1].Value = NumeroIdVacunado;
            }

            arParms[2] = new SqlParameter("@PrimerNombreVacunado", 22);
            if (string.IsNullOrEmpty(PrimerNombreVacunado))
            {
                arParms[2].Value = DBNull.Value;
            }
            else
            {
                arParms[2].Value = PrimerNombreVacunado;
            }

            arParms[3] = new SqlParameter("@SegundoNombreVacunado", 22);
            if (string.IsNullOrEmpty(SegundoNombreVacunado))
            {
                arParms[3].Value = DBNull.Value;
            }
            else
            {
                arParms[3].Value = SegundoNombreVacunado;
            }

            arParms[4] = new SqlParameter("@PrimerApellidoVacunado", 22);
            if (string.IsNullOrEmpty(PrimerApellidoVacunado))
            {
                arParms[4].Value = DBNull.Value;
            }
            else
            {
                arParms[4].Value = PrimerApellidoVacunado;
            }


            arParms[5] = new SqlParameter("@SegundoApellidoVacunado", 22);
            if (string.IsNullOrEmpty(SegundoApellidoVacunado))
            {
                arParms[5].Value = DBNull.Value;
            }
            else
            {
                arParms[5].Value = SegundoApellidoVacunado;
            }


            arParms[6] = new SqlParameter("@per_parInstitucion", 3);
            if (string.IsNullOrEmpty(per_parInstitucion))
            {
                arParms[6].Value = DBNull.Value;
            }
            else
            {
                arParms[6].Value = per_parInstitucion;
            }


            arParms[7] = new SqlParameter("@per_FechaNac", 31);
            arParms[7].Value = per_FechaNac;



            arParms[8] = new SqlParameter("@TipoIdentificacionMadre", 3);
            if (string.IsNullOrEmpty(TipoIdentificacionMadre))
            {
                arParms[8].Value = DBNull.Value;
            }
            else
            {
                arParms[8].Value = TipoIdentificacionMadre;
            }


            arParms[9] = new SqlParameter("@NumeroIdentificacionMadre", 22);
            if (string.IsNullOrEmpty(NumeroIdentificacionMadre))
            {
                arParms[9].Value = DBNull.Value;
            }
            else
            {
                arParms[9].Value = NumeroIdentificacionMadre;
            }


            arParms[10] = new SqlParameter("@PrimerNombreMadre", 22);
            if (string.IsNullOrEmpty(PrimerNombreMadre))
            {
                arParms[10].Value = DBNull.Value;
            }
            else
            {
                arParms[10].Value = PrimerNombreMadre;
            }


            arParms[11] = new SqlParameter("@SegundoNombreMadre", 22);
            if (string.IsNullOrEmpty(SegundoNombreMadre))
            {
                arParms[11].Value = DBNull.Value;
            }
            else
            {
                arParms[11].Value = SegundoNombreMadre;
            }


            arParms[12] = new SqlParameter("@PrimerApellidoMadre", 22);
            if (string.IsNullOrEmpty(PrimerApellidoMadre))
            {
                arParms[12].Value = DBNull.Value;
            }
            else
            {
                arParms[12].Value = PrimerApellidoMadre;
            }


            arParms[13] = new SqlParameter("@SegundoApellidoMadre", 22);
            if (string.IsNullOrEmpty(SegundoApellidoMadre))
            {
                arParms[13].Value = DBNull.Value;
            }
            else
            {
                arParms[13].Value = SegundoApellidoMadre;
            }


            arParms[14] = new SqlParameter("@grupoEtareo", 8);
            arParms[14].Value = grupoEtareo;


           DataSet dsPersona = SqlHelper.ExecuteDataset(cadena, "pa_SeleccionarPersonaBusqueda", arParms);
            List<PersonaEntity> personas = new List<PersonaEntity>();       

            foreach (DataRow dr in dsPersona.Tables[0].Rows)
            {
                PersonaEntity persona =new PersonaEntity();

                persona.per_Consecutivo = Convert.ToInt64(dr["per_Consecutivo"]);

                if (!dr["per_TipoId"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.per_TipoId = dr["per_TipoId"].ToString();
                }

                if (!dr["per_Id"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.per_Id = dr["per_Id"].ToString();
                }

                if (!dr["per_CertNacVivo"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.per_CertNacVivo = dr["per_CertNacVivo"].ToString();
                }

                if (!dr["per_CertDefuncion"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.per_CertDefuncion = dr["per_CertDefuncion"].ToString();
                }

                if (!dr["per_TipoIdM"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.per_TipoIdM = dr["per_TipoIdM"].ToString();
                }

                if (!dr["per_IdM"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.per_IdM = dr["per_IdM"].ToString();
                }

                if (!dr["per_NumeroHijoM"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.per_NumeroHijoM = short.Parse(dr["per_NumeroHijoM"].ToString());
                }

                if (!dr["per_nombre1"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.primerNombre = (dr["per_nombre1"].ToString());
                }

                if (!dr["per_nombre2"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.segundoNombre = dr["per_nombre2"].ToString();
                }

                if (!dr["per_apellido1"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.primerApellido = dr["per_apellido1"].ToString();
                }

                if (!dr["per_apellido2"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.segundoApellido = dr["per_apellido2"].ToString();
                }

                if (!dr["per_FechaNac"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.perFechaNac =DateTime.Parse(dr["per_FechaNac"].ToString());
                }

                if (!dr["per_parInstitucion"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.per_parInstitucion = dr["per_parInstitucion"].ToString();
                }

                if (!dr["per_FechaAlm"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.per_FechaAlm = DateTime.Parse(dr["per_FechaAlm"].ToString());
                }

                if (!dr["per_func"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.per_Func = dr["per_func"].ToString();
                }

                if (!dr["per_Estado"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.per_Estado = int.Parse(dr["per_Estado"].ToString());
                }

                if (!dr["per_Cni_id"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.cni_id = int.Parse(dr["per_Cni_id"].ToString());
                }

                if (!dr["per_idEtnia"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.etn_idEtnia = short.Parse(dr["per_idEtnia"].ToString());
                }

                if (!dr["per_IdGrupoPoblacional"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.gru_IdGrupo = dr["per_IdGrupoPoblacional"].ToString();
                }

                if (!dr["per_IdGenero"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.per_Genero = dr["per_IdGenero"].ToString();
                }

                if (!dr["per_IdGrupoSanguineo"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.perGrupoSanguineo = dr["per_IdGrupoSanguineo"].ToString();
                }

                if (!dr["per_IdRh"].GetType().ToString().Equals("System.DBNull"))
                {
                    persona.perRh = dr["per_IdRh"].ToString();
                }


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


        public string seleccionarPersonaIdentificacion(long per_Consecutivo)
        {
            string strReturn = "";
            List<SqlParameter> lParms = new List<SqlParameter>();
            var comi = Strings.Chr(34);
            lParms.Add(new SqlParameter("@per_Consecutivo", 0) { Value=per_Consecutivo}); 
            

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
    }
}
