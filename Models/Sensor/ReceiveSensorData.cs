using System;
using System.Net;
using System.Net.Sockets;
using static System.Console;
using System.Text;
using System.Net.NetworkInformation;

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
        Action<object> act = async obj =>
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

                ClientHandler hanlder = new ClientHandler(clientSocket);
                Thread t = new Thread(new ThreadStart(hanlder.chat));

                t.Start();

                /*
                // Socket clientSocket = server.AcceptSocket();
                // IPAddress ip = ((IPEndPoint)clientSocket.RemoteEndPoint).Address;
                // int index = 0;
                // foreach(var c in clientHanderList){
                //     IPAddress preIp = ((IPEndPoint)c.socket.RemoteEndPoint).Address;
                //     if(ip.Equals(preIp)){
                //         c.stream.Close();
                //         c.socket.Close();
                //         c.reader.Close();
                //         c.removeSensor();
        
                //         clientHanderList.Remove(c);
                //         // clientThread[index] = null;
                //         // clientThread.Remove(clientThread[index]);
                //         threads[0].Interrupt();
                //         Console.WriteLine("Remove client");
                //         if(clientHanderList.Count == 0) break;
                //     }
                //     index++;
                // }

                // Console.WriteLine("클라이언트 접속:" + IPAddress.Parse(ip.ToString()));

                // ClientHandler handler = new ClientHandler(clientSocket);
                // clientHanderList.Add(handler);

                // Thread t = new Thread(new ThreadStart(handler.chat));
                // clientThread.Add(t);
                // t.Start();
                */



                /*
                // bool existIp = false;
                // Socket socket = server.AcceptSocket();
                // IPAddress ip = IPAddress.Parse(((IPEndPoint)socket.RemoteEndPoint).Address.ToString());
                // foreach(var c in clientSocket){
                //     IPAddress preIp = IPAddress.Parse(((IPEndPoint)c.RemoteEndPoint).Address.ToString());
                //     if(ip.Equals(preIp)){
                //         Console.WriteLine("Exist socket");
                //         existIp = true;
                //     }
                // }
                // if(!existIp){
                //     clientSocket.Add(socket);

                //     WriteLine("클라이언트 접속:" + IPAddress.Parse(((IPEndPoint)clientSocket.Last().RemoteEndPoint).Address.ToString()));

                //     ClientHandler hanlder = new ClientHandler(clientSocket.Last());
                //     Thread t = new Thread(new ThreadStart(hanlder.chat));

                //     t.Start();
                // }
                */

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

class ClientHandler
{
    private ulong id = 0;
    public Socket socket = null;
    public NetworkStream stream = null;
    public StreamReader reader = null;
    public StreamWriter writer = null;
    public IPAddress ip;
    public IPAddress port;
    private SensorData sensorData;
    public ClientHandler(Socket socket)
    {
        this.socket = socket;
    }
    public void chat()
    {
        //클라이언트의 데이터를 읽고, 쓰기 위한 스트림을 만든다.
        Console.WriteLine("Chat start"); 
        stream = new NetworkStream(socket);
        Encoding encode = Encoding.GetEncoding("utf-8");
        reader = new StreamReader(stream, encode);
        // writer = new StreamWriter(stream, encode) { AutoFlush = true };

        int length = 0;
        string data = string.Empty;
        byte[] bytes = new byte[256];

        try
        {
            while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                WriteLine("TCP Byte length : " + length);
                data = Encoding.Default.GetString(bytes, 0, length);
                WriteLine("TCP Received bytes : ");
                foreach (var v in bytes)
                {
                    Console.Write(" " + v);
                }
                WriteLine("\nReceived string : " + data);

                getSensorData(bytes);
                // byte[] message = Encoding.Default.GetBytes(data);
                // stream.Write(message, 0, message.Length);
            }
        }
        catch
        {
            Console.WriteLine("Stream read error", ConsoleColor.Red);
        }
        finally
        {
            stream.Close();
            socket.Close();
            reader.Close();
            // writer.Close();
            removeSensor();
            WriteLine("Socket closed");
        }
    }

    public void getSensorData(byte[] buf)
    {
        if(id == 0)
        {
            id = (ulong)((buf[4] << 40) | (buf[5] << 32) | (buf[6] << 24)
                    | (buf[7] << 16) | (buf[8] << 8) | buf[9]);
            sensorData = new SensorData(id);
            sensorData.ip = IPAddress.Parse(((IPEndPoint)socket.RemoteEndPoint).Address.ToString());
            sensorData.port = IPAddress.Parse(((IPEndPoint)socket.RemoteEndPoint).Port.ToString());
            ReceiveSensorData.sensorList.Add(sensorData);
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
        // if (ReceiveSensorData.sensorList.Count != 0)
        // {
        //     foreach (var s in ReceiveSensorData.sensorList)
        //     {
        //         if (s.id == id && s.ip.Equals(ip) && s.port.Equals(port))
        //         {
        //             ReceiveSensorData.sensorList.Remove(s);
        //             if(ReceiveSensorData.sensorList.Count == 0) break;
        //             id = 0;
        //         }
        //     }
        // }
        ReceiveSensorData.sensorList.Remove(sensorData);
        Console.WriteLine("Removed sensor");
    }
}

class SensorData{
    public ulong id;
    public int smoke;
    public int temp;
    public int gas;
    public IPAddress ip;
    public IPAddress port;
    public SensorData(ulong id)
    {
        this.id = id;
    }
}
