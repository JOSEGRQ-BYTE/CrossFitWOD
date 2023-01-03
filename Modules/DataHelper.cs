using System;
using System.Data;
using System.Data.SqlClient;

namespace CrossFitWOD.Modules
{
	public static class DataHelper
	{
        public static DataSet ExecuteStoredProcedure(string connectionString,string storedProcedure, SqlParameter[] parameters)
		{
            DataSet ds = new DataSet(); ;
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedure, sqlConn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    // Add Parameters
                    if (parameters is not null)
                        sqlCommand.Parameters.AddRange(parameters.ToArray());


                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlDataAdapter.Fill(ds);
                    }
                }
            }
            return ds;

        }
	}
}

