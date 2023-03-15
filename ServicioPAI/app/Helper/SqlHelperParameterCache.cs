using Microsoft.Net.Http.Headers;
using Microsoft.VisualBasic;
using System.Data.SqlClient;

namespace ServicioPAI.Helper
{
    public class SqlHelperParameterCache
    {
        private static System.Collections.Hashtable paramCache = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }

        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            SqlParameter[] sqlParameters = null;

            if ((connectionString == null || connectionString.Length == 0))
                throw new System.ArgumentNullException("connectionString");
            SqlConnection connection = new SqlConnection();
            try
            {
                connection = new SqlConnection(connectionString);
                sqlParameters = GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }

            return sqlParameters;
        } // GetSpParameterSet

        private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if ((connection == null))
                throw new System.ArgumentNullException("connection");

            SqlParameter[] cachedParameters=null;
            string hashKey;

            if ((spName == null || spName.Length == 0))
                throw new System.ArgumentNullException("spName");

            hashKey = connection.ConnectionString + ":" + spName + Interaction.IIf(includeReturnValueParameter == true, ":include ReturnValue Parameter", "").ToString();

            //cachedParameters = (SqlParameter[])paramCache(hashKey);
            //paramCache.CopyTo(cachedParameters, 0);
            if ((cachedParameters == null))
            {
                SqlParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                //paramCache(hashKey) = spParameters;
                //foreach (var item in spParameters)
                //{
                //    paramCache(hashKey) = item;
                //}
                cachedParameters = spParameters;
            }

            return CloneParameters(cachedParameters);
        } // GetSpParameterSet

        // Deep copy of cached SqlParameter array
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            int i;
            int j = originalParameters.Length - 1;
            SqlParameter[] clonedParameters = new SqlParameter[j + 1];

            for (i = 0; i <= j; i++)
                clonedParameters[i] = (SqlParameter)(System.ICloneable)originalParameters[i];

            return clonedParameters;
        } // CloneParameters


        private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter, params object[] parameterValues)
        {
            if ((connection == null))
                throw new System.ArgumentNullException("connection");
            if ((spName == null || spName.Length == 0))
                throw new System.ArgumentNullException("spName");
            SqlCommand cmd = new SqlCommand(spName, connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter[] discoveredParameters;
            connection.Open();
            SqlCommandBuilder.DeriveParameters(cmd);
            connection.Close();
            if (!includeReturnValueParameter)
                cmd.Parameters.RemoveAt(0);

            discoveredParameters = new SqlParameter[cmd.Parameters.Count - 1 + 1];
            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // Init the parameters with a DBNull value           
            foreach (var discoveredParameter in discoveredParameters)
                discoveredParameter.Value = System.DBNull.Value;

            return discoveredParameters;
        } // DiscoverSpParameterSet


    }
}
