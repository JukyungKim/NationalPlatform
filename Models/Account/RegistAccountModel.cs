using Npgsql;

namespace NationalPlatform.Models;
public class RegistAccountModel
{

    public static void InsertAccount(string id,
    string password, string name, string authority, string phone_number, string district)
    {
        using (var conn = new NpgsqlConnection(
                    "host=localhost;username=postgres;password=1234;database=nationaldb"))
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = String.Format("INSERT INTO account VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    id, password, name, authority, phone_number, district);
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine(cmd.CommandText);
                        while (reader.Read())
                        {
                            Console.Write(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public static void ReadAccount()
    {
        using (var conn = new NpgsqlConnection(
                    "host=localhost;username=postgres;password=1234;database=nationaldb"))
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = String.Format("SELECT * FROM account ORDER BY id DESC");
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine(cmd.CommandText);
                        while (reader.Read())
                        {
                            Console.Write(reader.GetString(0) + " ");
                            Console.Write(reader.GetString(1) + " ");
                            Console.Write(reader.GetString(2) + " ");
                            Console.Write(reader.GetString(3) + " ");
                            Console.Write(reader.GetString(4) + " ");
                            Console.Write(reader.GetString(5) + " ");
                            Console.Write("\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
