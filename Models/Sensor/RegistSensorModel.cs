using Npgsql;

namespace NationalPlatform.Models;
public class RegistSensorModel
{
    public void InsertToTable()
    {

    }

    static public void createTable()
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
    static public bool CheckSensorId(string id, string plan)
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
                    cmd.CommandText = String.Format("select * from sensor_info where id='{0}';", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine(cmd.CommandText);
                        // while (reader.Read())
                        // {
                        //     Console.Write(reader.GetString(0));
                        // }
                        if(reader.Read()){
                            Console.WriteLine("Exist sensor id");
                            return true;
                        }
                        else{
                            Console.WriteLine("Not exist sensor id");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        return false;
    }

    static public bool CheckSensorNum(string id, string plan)
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
                    cmd.CommandText = String.Format("select * from sensor_info where plan_id='{0}';", plan);
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine(cmd.CommandText);
                        int sensorCount = 0;
                        while (reader.Read())
                        {
                            sensorCount++;
                            Console.WriteLine("Check sensor num : " + reader.GetString(0) + ", count:" + sensorCount);
                            
                        }
                        if (sensorCount > 12)
                        {
                            Console.WriteLine("Sensor num 12 over");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Sensor num 12 under");
                            return false;
                        }
                        // if(reader.Read()){
                        //     Console.WriteLine("Sensor num 12 over");
                        //     return true;
                        // }
                        // else{
                        //     Console.WriteLine("Sensor num 12 under");
                        //     return false;
                        // }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        return false;
    }

    

    static public void InsertSensorInfo(string id, string address, string name, string dong, string floor, string ho, string num, string plan, string x, string y)
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
                    cmd.CommandText = String.Format("INSERT INTO sensor_info VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');",
                    id, address, name, dong, floor, ho, num, plan, x, y);
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

    static public void RemoveSensor(string id)
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
                    cmd.CommandText = String.Format("DELETE FROM sensor_info WHERE id='{0}';", id);
                    Console.WriteLine("Remove Sensor : {0}", cmd.CommandText);
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

    static public void ReadSensorInfo()
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
                            Console.Write(reader.GetString(7) + " ");
                            Console.Write(reader.GetString(8) + " ");
                            Console.Write(reader.GetString(9) + " ");
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

    static public void SearchSensor(string planId, List<string> sensorId)
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
                    cmd.CommandText = String.Format("SELECT * FROM sensor_info WHERE plan_id='{0}'", planId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine(cmd.CommandText);
                        while (reader.Read())
                        {
                            sensorId.Add(uint.Parse(reader.GetString(0), System.Globalization.NumberStyles.AllowHexSpecifier).ToString());
                            // sensorId.Add(reader.GetString(0));
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
