using Npgsql;
using System.Drawing;
 
namespace NationalPlatform.Models;
public class RegistPlanModel
{
    static readonly string imgPath = "images/test.png";
    public void InsertToTable()
    {

    }

    public static void InsertImage()
    {
        FileStream fileStream = new FileStream("D:\\img.png", FileMode.Open, FileAccess.Read);
        BinaryReader reader = new BinaryReader(new BufferedStream(fileStream));
        byte[] imgByte = reader.ReadBytes(Convert.ToInt32(fileStream.Length));

        string connString = "host=localhost;username=postgres;password=1234;database=nationaldb";
        using (var conn = new NpgsqlConnection(connString))
        {
            string sQL = "INSERT INTO plan (plan_image_name, plan_image) VALUES('65', @Image)";
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
    public static string ReadImage()
    {
        string connString = "host=localhost;username=postgres;password=1234;database=nationaldb";
        using (var conn = new NpgsqlConnection(connString))
        {
            string sQL = "SELECT plan_image from plan WHERE plan_image_name = '65'";
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
                        bitmap.Save("wwwroot/" + imgPath);
                        Console.WriteLine("Image loaded");

                        return imgPath;
                    }
                }
            }
        }
        return "";
    }

    public static List<byte> LoadPlanInfoList()
    {
        List<byte> infoList = new List<byte>();

        infoList.Add(10);
        infoList.Add(20);
        infoList.Add(30);

        return infoList;
    }
}

