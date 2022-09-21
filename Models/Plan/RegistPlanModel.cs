using Npgsql;
using System.Drawing;
using System.Text;

namespace NationalPlatform.Models;
public class RegistPlanModel
{
    static readonly string IMG_PATH = "images/test.png";
    public void InsertToTable()
    {

    }

    public static void InsertImage(IFormFile file, string address, string plan_image_name, 
        string building_name, string dong, string floor, string ho)
    {
        // FileStream fileStream = new FileStream("D:\\img.png", FileMode.Open, FileAccess.Read);
        // BinaryReader reader = new BinaryReader(new BufferedStream(fileStream));
        // byte[] imgByte = reader.ReadBytes(Convert.ToInt32(fileStream.Length));

        var ms = new MemoryStream();
        file.CopyTo(ms);
        byte[] imgByte = ms.ToArray();

        string connString = "host=localhost;username=postgres;password=1234;database=nationaldb";
        using (var conn = new NpgsqlConnection(connString))
        {
            // string sQL = "INSERT INTO plan (plan_image_name, plan_image) VALUES('70', @Image)";
            string sQL = String.Format("INSERT INTO plan (plan_image_name, plan_image, building_name, dong, floor, ho, address) VALUES('{0}', @Image, '{1}', '{2}', '{3}', '{4}', '{5}')",
                plan_image_name, building_name, dong, floor, ho, address);
            using (var command = new NpgsqlCommand(sQL, conn))
            {
                NpgsqlParameter param = command.CreateParameter();
                param.ParameterName = "@Image";
                param.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bytea;
                param.Value = imgByte;
                command.Parameters.Add(param);

                conn.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Image saved");
            }
        }
    }

    static public bool CheckPlanId(string id)
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
                    cmd.CommandText = String.Format("select * from plan where plan_image_name='{0}';", id);
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

    static public void RemovePlan(string planName)
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
                    cmd.CommandText = String.Format("DELETE FROM plan WHERE plan_image_name='{0}';", planName);
                    Console.WriteLine("Remove Plan : {0}", cmd.CommandText);
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
    public static string ReadImage(string planImageName)
    {
        Console.WriteLine("Read image");
        string connString = "host=localhost;username=postgres;password=1234;database=nationaldb";
        using (var conn = new NpgsqlConnection(connString))
        {
            string sQL = string.Format("SELECT plan_image from plan WHERE plan_image_name = '{0}'", planImageName);
            using (var command = new NpgsqlCommand(sQL, conn))
            {
                byte[] productImageByte = null;
                conn.Open();
                var rdr = command.ExecuteReader();
                if (rdr.Read())
                {
                    productImageByte = (byte[])rdr[0];
                }
                rdr.Close();
                if (productImageByte != null)
                {
                    using (MemoryStream productImageStream = new System.IO.MemoryStream(productImageByte))
                    {
                        ImageConverter imageConverter = new System.Drawing.ImageConverter();

                        // pictureBox1.Image = imageConverter.ConvertFrom(productImageByte) as System.Drawing.Image;
                        Bitmap bitmap = new Bitmap(imageConverter.ConvertFrom(productImageByte) as System.Drawing.Image);
                        bitmap.Save("wwwroot/" + IMG_PATH);
                        Console.WriteLine("Image loaded");

                        return IMG_PATH;
                    }
                }
            }
        }
        return "";
    }

    public static byte[] LoadPlanImage(string planId)
    {
        string connString = "host=localhost;username=postgres;password=1234;database=nationaldb";
        byte[] productImageByte = null;

        Console.WriteLine("Load plan image : " + planId);

        using (var conn = new NpgsqlConnection(connString))
        {
            string sQL = string.Format("SELECT plan_image from plan WHERE plan_image_name='{0}'", planId);//planId);
            Console.WriteLine(sQL);
            using (var command = new NpgsqlCommand(sQL, conn))
            {
                conn.Open();
                var rdr = command.ExecuteReader();

                if (rdr.Read() && rdr[0] != DBNull.Value)
                {
                    productImageByte = (byte[])rdr[0];
                }
                rdr.Close();
            }
        }
        if(productImageByte == null){
            productImageByte = Encoding.UTF8.GetBytes("is empty");
        }
        // Console.WriteLine(Encoding.Default.GetString(productImageByte));

        return productImageByte;
    }

    public static List<byte> LoadPlanInfoList()
    {
        List<byte> infoList = new List<byte>();

        using (var conn = new NpgsqlConnection(
            "host=localhost;username=postgres;password=1234;database=nationaldb"))
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = String.Format("SELECT * FROM plan ORDER BY plan_image_name DESC");
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine(cmd.CommandText);
                        infoList.Add(4);

                        while (reader.Read())
                        {
                            
                            // ConvertToByte(infoList, reader.GetString(0));
                            
                            if (reader["plan_image_name"] == DBNull.Value)
                            {
                                ConvertToByte(infoList, "");
                            }
                            else
                            {
                                ConvertToByte(infoList, reader.GetString(0));
                            }

                            if (reader["address"] == DBNull.Value)
                            {
                                ConvertToByte(infoList, "");
                            }
                            else
                            {
                                ConvertToByte(infoList, reader.GetString(2));
                            }

                            if (reader["building_name"] == DBNull.Value)
                            {
                                ConvertToByte(infoList, "");
                            }
                            else
                            {
                                ConvertToByte(infoList, reader.GetString(3));
                            }

                            if (reader["dong"] == DBNull.Value)
                            {
                                ConvertToByte(infoList, "");
                            }
                            else
                            {
                                ConvertToByte(infoList, reader.GetString(4));
                            }

                            if (reader["floor"] == DBNull.Value)
                            {
                                ConvertToByte(infoList, "");
                            }
                            else
                            {
                                ConvertToByte(infoList, reader.GetString(5));
                            }

                            if (reader["ho"] == DBNull.Value)
                            {
                                ConvertToByte(infoList, "");
                            }
                            else
                            {
                                ConvertToByte(infoList, reader.GetString(6));
                            }
                            Console.WriteLine("Size:" + infoList.Count);

                            // Console.Write(reader.GetString(0) + " ");
                            // // Console.Write(reader.GetString(1) + " ");
                            // Console.Write(reader.GetString(2) + " ");
                            // Console.Write(reader.GetString(3) + " ");
                            // Console.Write(reader.GetString(4) + " ");
                            // Console.Write(reader.GetString(5) + " ");
                            // Console.Write(reader.GetString(6) + " ");
                            Console.Write("\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }   
        }

        // infoList.Add(10);
        // infoList.Add(20);
        // infoList.Add(30);

        return infoList;
    }

    private static void ConvertToByte(List<byte> list, string data)
    {
        byte[] buf = Encoding.UTF8.GetBytes(data);                
        int index = 0;
        foreach(var v in buf){
            list.Add(v);
        }
        for(int i = buf.Length; i < 30; i++){
            list.Add(0);
        }
    }
}

