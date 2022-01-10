using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
public class DBManager
{
    private static string ConnStr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
    public static DataTable ExecuteDataTableWithParamiter(string Query, CommandType commandType, List<SqlParameter> Parameter)
    {
        DataTable DT = new DataTable();
        using (SqlConnection sqlcon = new SqlConnection((ConnStr)))
        {
            using (SqlCommand Command = new SqlCommand(Query, sqlcon))
            {
                sqlcon.Open();
                Command.CommandType = commandType;
                foreach (SqlParameter Pr in Parameter)
                {
                    Command.Parameters.Add(Pr);
                }
                using (var Obj = Command.ExecuteReader())
                {
                    DT.Load(Obj);
                    sqlcon.Close();
                    return DT;
                }
            }
        }
    }
    public static string ExecuteScalar(string Query, CommandType commandType, List<SqlParameter> Parameter)
    {
        DataTable DT = new DataTable();
        using (SqlConnection sqlcon = new SqlConnection((ConnStr)))
        {
            using (SqlCommand Command = new SqlCommand(Query, sqlcon))
            {
                sqlcon.Open();
                Command.CommandType = commandType;
                foreach (SqlParameter Pr in Parameter)
                {
                    Command.Parameters.Add(Pr);
                }
                string Obj2 = Command.ExecuteScalar().ToString();
                sqlcon.Close();
                return Obj2;
            }
        }
    }
    public static DataSet ExecuteDataSetWithParamiter(string Query, CommandType commandType, List<SqlParameter> Parameter)
    {
        using (SqlConnection Connection = new SqlConnection(ConnStr))
        {
            using (SqlCommand Command = new SqlCommand(Query, Connection))
            {
                Connection.Open();
                Command.CommandType = commandType;
                foreach (SqlParameter Pr in Parameter)
                {
                    Command.Parameters.Add(Pr);
                }
                SqlDataAdapter DataAdapter = new SqlDataAdapter(Command);
                DataSet Dataset = new DataSet();
                DataAdapter.Fill(Dataset);
                Connection.Close();
                return Dataset;
            }
        }
    }
}

