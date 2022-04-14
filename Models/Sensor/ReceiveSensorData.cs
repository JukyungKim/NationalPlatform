using System;
using System.Net;
using System.Net.Sockets;
using static System.Console;
using System.Text;

namespace NationalPlatform.Models;

class ReceiveSensorData
{
    public static List<SensorData> sensorList = new List<SensorData>();

    private static TcpListener server;

    public static async void Start()
    {
        Test();
        await ServerTask();
    }

    public static Task ServerTask()
    {
        int numThreads = 1;
        Action<object> act = obj =>
        {
            // IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1212);
            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Any, 1212);
            server = new TcpListener(serverAddress);
            server.Start();
            WriteLine("서버 시작됨");

            while (true)
            {
                Socket clientSocket = server.AcceptSocket();

                WriteLine("클라이언트 접속");

                ClientHadler hanlder = new ClientHadler(clientSocket);
                Thread t = new Thread(new ThreadStart(hanlder.chat));

                t.Start();
            }
        };
        Task task = new Task(act, "tcp");

        task.Start();

        return task;
    }

    public static void ServerThread(object obj)
    {
        // while (true)
        // {
        //     TcpClient client = server.AcceptTcpClient();

        //     WriteLine("클라이언트 접속");

        //     NetworkStream stream = client.GetStream();

        //     int length = 0;
        //     string data = string.Empty;
        //     byte[] bytes = new byte[256];


        //     while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
        //     {

        //         ReceiveData(bytes);
        //         data = Encoding.Default.GetString(bytes, 0, length);
        //         WriteLine(data);

        //         // byte[] message = Encoding.Default.GetBytes(data);
        //         // stream.Write(message, 0, message.Length);
        //     }

        //     stream.Close();
        //     client.Close();
        // }

        // server.Stop();
        // WriteLine("서버 종료");
    }

    public static void ReceiveData(byte[] data)
    {
        // int smoke;
        // int temp;
        // int gas;

        // smoke = (data[14] << 24) | (data[15] << 16) | (data[16] << 8) | data[17];
        // temp = (data[18] << 24) | (data[19] << 16) | (data[20] << 8) | data[21];
        // gas = (data[22] << 24) | (data[23] << 16) | (data[24] << 8) | data[25];

        // Console.WriteLine("Sensor data (smoke),(temp),(gas) : {0}, {1}, {2}", smoke, temp, gas);
    }

    public static async void Test()
    {
        int value1;

        byte[] b1 = {1, 2, 3, 4};

        value1 = (b1[0] << 24) | (b1[1] << 16) | (b1[2] << 8) | b1[3];

        Console.WriteLine("Test : {0:X}", value1);

        long t = 288;
        byte[] bytes = BitConverter.GetBytes(t);
        Console.WriteLine("Bytes length : " + bytes.Length);

        foreach(var b in bytes){
            Console.WriteLine(b);
        }
        // hex string to int
        uint m = uint.Parse("abc", System.Globalization.NumberStyles.AllowHexSpecifier);
        Console.WriteLine(m);
    }
}

class ClientHadler
{
    private ulong id = 0;
    Socket socket = null;
    NetworkStream stream = null;
    StreamReader reader = null;
    StreamWriter writer = null;

    public ClientHadler(Socket socket)
    {
        this.socket = socket;
    }
    public void chat()
    {
        //클라이언트의 데이터를 읽고, 쓰기 위한 스트림을 만든다. 
        stream = new NetworkStream(socket);
        Encoding encode = Encoding.GetEncoding("utf-8");
        reader = new StreamReader(stream, encode);
        // writer = new StreamWriter(stream, encode) { AutoFlush = true };

        int length = 0;
        string data = string.Empty;
        byte[] bytes = new byte[256];

        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
        {
            WriteLine("Byte length : " + length);
            data = Encoding.Default.GetString(bytes, 0, length);
            WriteLine("Received bytes : ");
            foreach(var v in bytes){
                Console.Write(" " + v);
            }
            WriteLine("\nReceived string : " + data);

            getSensorData(bytes);
            // byte[] message = Encoding.Default.GetBytes(data);
            // stream.Write(message, 0, message.Length);
        }

        stream.Close();
        socket.Close();
        reader.Close();
        // writer.Close();
        removeSensor();
        WriteLine("Socket closed");
    }

    public void getSensorData(byte[] buf)
    {
        if(id == 0)
        {
            id = (ulong)((buf[4] << 40) | (buf[5] << 32) | (buf[6] << 24)
                    | (buf[7] << 16) | (buf[8] << 8) | buf[9]);
            ReceiveSensorData.sensorList.Add(new SensorData(id));
        }
        
        foreach (var s in ReceiveSensorData.sensorList)
        {
            if (s.id == id)
            {
                s.smoke = (buf[14] << 24) | (buf[15] << 16) | (buf[16] << 8) | buf[17];
                s.temp = (buf[18] << 24) | (buf[19] << 16) | (buf[20] << 8) | buf[21];
                s.gas = (buf[22] << 24) | (buf[23] << 16) | (buf[24] << 8) | buf[25];
                
                Console.WriteLine("Id smoke temp gas : {0}, {1}, {2}, {3}", s.id, s.smoke, s.temp, s.gas);

                break;
            }
        }
    }

    public void setSensor()
    {

    }
    public void removeSensor()
    {
        foreach(var s in ReceiveSensorData.sensorList){
            if(s.id == id){
                ReceiveSensorData.sensorList.Remove(s);
                id = 0;
            }
        }
        Console.WriteLine("Removed sensor");
    }
}

class SensorData{
    public ulong id;
    public int smoke;
    public int temp;
    public int gas;
    public SensorData(ulong id)
    {
        this.id = id;
    }
}
