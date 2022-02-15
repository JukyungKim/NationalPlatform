using Npgsql;

namespace NationalPlatform.Models;
public class RegistSensorModel
{
    public void InsertToTable()
    {

    }

    public static void createTable()
    {
        using (var conn = new NpgsqlConnection(
            "host=localhost;username=postgres;password=1234;database=test"))
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText =
                    "create table test2(id varchar(20), name varchar(20));";
                    using (var reader = cmd.ExecuteReader())
                    {
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

    public static void InsertSensorInfo(string id, string address, string name, string dong, string floor, string ho, string num)
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
                    cmd.CommandText = String.Format("INSERT INTO sensor_info VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');",
                    id, address, name, dong, floor, ho, num);
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

    public static void ReadSensorInfo()
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
                    cmd.CommandText = String.Format("SELECT * FROM sensor_info ORDER BY id DESC");
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
                            Console.Write(reader.GetString(6) + " ");
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
