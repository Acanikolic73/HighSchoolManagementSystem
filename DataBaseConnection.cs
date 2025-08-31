using System.Data;
using System.Data.SQLite;

public class DataBaseConnection
{
    public string connectionString = @"Data Source=C:\Users\Aleksandar\source\repos\HighSchoolManagement\Baza.db";
    public string query;
    private SQLiteDataAdapter da;

    public DataSet Data()
    {
        using (SQLiteConnection con = new SQLiteConnection(connectionString))
        {
            con.Open();
            da = new SQLiteDataAdapter(query, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
    }

    public void Update(DataSet ds)
    {
        SQLiteCommandBuilder cb = new SQLiteCommandBuilder(da);
        da.Update(ds.Tables[0]);
    }

    public int ExecuteNonQuery(string sql)
    {
        using (SQLiteConnection con = new SQLiteConnection(connectionString))
        {
            con.Open();
            using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
            {
                return cmd.ExecuteNonQuery();
            }
        }
    }

    public object ExecuteScalar(string sql)
    {
        using (SQLiteConnection con = new SQLiteConnection(connectionString))
        {
            con.Open();
            using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
            {
                return cmd.ExecuteScalar();
            }
        }
    }
}
