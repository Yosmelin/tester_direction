using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace ServicioPAI.Helper
{
    public class SqlHelper
    {


        #region "ExecuteDataset"
        public static System.Data.DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new System.ArgumentNullException("connectionString");
            if ((spName == null || spName.Length == 0))
                throw new System.ArgumentNullException("spName");
            SqlParameter[] commandParameters;

            // If we receive parameter values, we need to figure out where they go
            if (!(parameterValues == null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteDataset(connectionString, System.Data.CommandType.StoredProcedure, spName, commandParameters);
            }
            else
                return ExecuteDataset(connectionString, System.Data.CommandType.StoredProcedure, spName);
        } // ExecuteDataset

        public static System.Data.DataSet ExecuteDataset(string connectionString, System.Data.CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new System.ArgumentNullException("connectionString");

            // Create & open a SqlConnection, and dispose of it after we are done
            SqlConnection connection = new SqlConnection();
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }
        } // ExecuteDataset


        public static System.Data.DataSet ExecuteDataset(SqlConnection connection, System.Data.CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connection == null))
                throw new System.ArgumentNullException("connection");
            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            System.Data.DataSet ds = new System.Data.DataSet();
            SqlDataAdapter dataAdatpter = new SqlDataAdapter();
            bool mustCloseConnection = false;

            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);

            try
            {
                // Create the DataAdapter & DataSet
                dataAdatpter = new SqlDataAdapter(cmd);

                // Fill the DataSet using default values for DataTable names, etc
                dataAdatpter.Fill(ds);

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();
            }
            finally
            {
                if ((dataAdatpter != null))
                    dataAdatpter.Dispose();
            }
            if ((mustCloseConnection))
                connection.Close();

            // Return the dataset
            return ds;
        } // ExecuteDataset
        #endregion


        #region "ExecuteNonQuery"
        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new System.ArgumentNullException("connectionString");
            if ((spName == null || spName.Length == 0))
                throw new System.ArgumentNullException("spName");

            SqlParameter[] commandParameters;

            // If we receive parameter values, we need to figure out where they go
            if (!(parameterValues == null) && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)

                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(connectionString, System.Data.CommandType.StoredProcedure, spName, commandParameters);
            }
            else
                return ExecuteNonQuery(connectionString, System.Data.CommandType.StoredProcedure, spName);
        } // ExecuteNonQuery


        public static int ExecuteNonQuery(string connectionString, System.Data.CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        public static int ExecuteNonQuery(string connectionString, System.Data.CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null || connectionString.Length == 0))
                throw new System.ArgumentNullException("connectionString");
            // Create & open a SqlConnection, and dispose of it after we are done
            SqlConnection connection = new SqlConnection();
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
            finally
            {
                if (!(connection == null))
                    connection.Dispose();
            }
        } // ExecuteNonQuery


        public static int ExecuteNonQuery(SqlConnection connection, System.Data.CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connection == null))
                throw new System.ArgumentNullException("connection");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            int retval;
            bool mustCloseConnection = false;

            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Finally, execute the command
            retval = cmd.ExecuteNonQuery();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            if ((mustCloseConnection))
                connection.Close();

            return retval;
        } // ExecuteNonQuery

        #endregion




        #region "private methods, variables, and constructors"
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, System.Data.CommandType commandType, string commandText, SqlParameter[] commandParameters, ref bool mustCloseConnection)
        {
            if ((command == null))
                throw new System.ArgumentNullException("command");
            if ((commandText == null || commandText.Length == 0))
                throw new System.ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                mustCloseConnection = true;
            }
            else
                mustCloseConnection = false;

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it.
            if (transaction != null)
            {
                if (transaction.Connection == null)
                    throw new System.ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (!(commandParameters == null))
                AttachParameters(command, commandParameters);
            return;
        } // PrepareCommand

        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null)
                throw new System.ArgumentNullException("command");
            if (!(commandParameters == null))
            {

                foreach (SqlParameter p in commandParameters)
                {
                    if (!(p == null))
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == System.Data.ParameterDirection.InputOutput || p.Direction == System.Data.ParameterDirection.Input) && p.Value == null)
                            p.Value = System.DBNull.Value;

                        command.Parameters.Add(new SqlParameter(p.ParameterName, p.SqlDbType, p.Size)).Value = p.Value;
                    }
                }
            }
        } // AttachParameters

        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            int i;
            int j;

            if ((commandParameters == null) && (parameterValues == null))
                // Do nothing if we get no data
                return;

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
                throw new System.ArgumentException("Parameter count does not match Parameter Value count.");

            // Value array
            j = commandParameters.Length - 1;
            for (i = 0; i <= j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is System.Data.IDbDataParameter)
                {
                    System.Data.IDbDataParameter paramInstance = (System.Data.IDbDataParameter)parameterValues[i];
                    if ((paramInstance.Value == null))
                        commandParameters[i].Value = System.DBNull.Value;
                    else
                        commandParameters[i].Value = paramInstance.Value;
                }
                else if ((parameterValues[i] == null))
                    commandParameters[i].Value = System.DBNull.Value;
                else
                    commandParameters[i].Value = parameterValues[i];
            }
        } // AssignParameterValues
        #endregion





    }
}
