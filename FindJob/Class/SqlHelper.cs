using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FindJob.Class
{
    public class SqlHelper
    {
        private static string _connStr = ConfigurationManager.ConnectionStrings["AliSql"].ConnectionString;

        public static int ExcuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExcuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static DataTable ExcuteDataTable(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    //DataSet dataSet = new DataSet();
                    DataTable dataTable = new DataTable();
                    SqlDataAdapter adpter = new SqlDataAdapter(cmd);
                    adpter.Fill(dataTable);
                    return dataTable;
                }
            }
        }
    }
}