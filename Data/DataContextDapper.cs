using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Data
{
    class DataContextDapper
    {
        private readonly IConfiguration _config;

        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }

        // Versi pakai parameter (biar bisa query yang aman pakai @param)
        public IEnumerable<T> LoadData<T>(string sql, object? parameters = null)
        {
            IDbConnection dbConnection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );
            return dbConnection.Query<T>(sql, parameters);
        }

        // Versi pakai parameter (biar bisa query yang aman pakai @param)
        public T LoadDataSinggle<T>(string sql, object? parameters = null)
        {
            IDbConnection dbConnection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );
            return dbConnection.QuerySingle<T>(sql, parameters);
        }

        public bool ExecuteSql(string sql, object? parameters = null)
        {
            IDbConnection dbConnection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );
            return dbConnection.Execute(sql, parameters) > 0;
        }

        public int ExecuteSqlWithRowCount(string sql, object? parameters = null)
        {
            IDbConnection dbConnection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );
            return dbConnection.Execute(sql, parameters);
        }

        public bool ExecuteSqlWithParameter(string sql, List<SqlParameter> parameters)
        {
            SqlCommand commandWithParams = new SqlCommand(sql);
            foreach (SqlParameter parameter in parameters)
            {
                commandWithParams.Parameters.Add(parameter);
            }

            SqlConnection dbConnection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );
            dbConnection.Open();

            commandWithParams.Connection = dbConnection;

            int rowsAffected = commandWithParams.ExecuteNonQuery();

            dbConnection.Close();

            return rowsAffected > 0;
        }
    }
}
