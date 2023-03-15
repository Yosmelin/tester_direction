using Dapper;
using Microsoft.Data.SqlClient;
using ServicioPAI.Entidades;
using ServicioPAI.Helper;
using ServicioPAI.Modelos;
using System.Data;

namespace ServicioPAI.Servicios
{
    public interface IRepositorioLog
    {
        void insertarLog(string metodo, string detalleError, SqlParameter[] arParms);
        void insertarLogBD(string metodo, string detalleError, SqlParameter[] arParms);
    }
    public class RepositorioLog: IRepositorioLog
    {
        private readonly string connectionString;
        
        public RepositorioLog(IConfiguration configuration) 
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");            
        }

        public void insertarLog(string metodo, string detalleError, SqlParameter[] arParms)
        {
            try
            {                
                const string fic = Constantes.rutaLog + "logServicioWebPAI4.txt";

                if (!File.Exists(fic))
                {
                    System.IO.FileStream oArchivo = System.IO.File.Create(fic);
                    oArchivo.Close();
                    oArchivo.Dispose();
                }

                System.IO.StreamWriter sw = new StreamWriter(fic, true);
                sw.WriteLine("Metodo: " + metodo + ";PosibleError:" + detalleError + ";Fecha: " + DateTime.Now.ToString());
                if (!(arParms == null))
                {
                    string lineaParametro = string.Empty;
                    string lineaDatos = string.Empty;
                    foreach (SqlParameter param in arParms)
                    {
                        lineaParametro += param.ParameterName.ToString() + ";";
                        lineaDatos += param.Value.ToString() + ";";
                    }
                    sw.WriteLine(lineaParametro);
                    sw.WriteLine(lineaDatos);
                    sw.WriteLine("");
                }

                sw.Close();
                sw.Dispose();
            }
            catch (Exception ex)
            {
            }
        }


        public void insertarLogBD(string metodo, string detalleError, SqlParameter[] arParms)
        {
            try
            {
                string lineaParametro = string.Empty;
                string lineaDatos = string.Empty;
                if (!(arParms == null))
                {
                    for (int i = 0; i <= 3; i++)
                    {
                        lineaParametro += arParms[i].ParameterName.ToString() + ";";
                        lineaDatos += arParms[i].Value.ToString() + ";";
                    }
                }

                string cadena = connectionString;

                SqlParameter[] arParmsInterno = new SqlParameter[26];

                arParmsInterno[0] = new SqlParameter("@metodo", 12);
                arParmsInterno[0].Value = metodo;

                arParmsInterno[1] = new SqlParameter("@detalle", 12);
                arParmsInterno[1].Value = detalleError;

                arParmsInterno[2] = new SqlParameter("@lineaParametros", 12);
                arParmsInterno[2].Value = lineaParametro;

                arParmsInterno[3] = new SqlParameter("@lineaDatos", 12);
                arParmsInterno[3].Value = lineaDatos;

                SqlHelper.ExecuteNonQuery(cadena, "pa_InsertarLogServicioWeb", arParmsInterno);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
