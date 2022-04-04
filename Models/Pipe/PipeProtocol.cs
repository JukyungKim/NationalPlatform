
using System.Text;
using NationalPlatform.Models;

public class PipeProtocol{
    enum EProtocol
    {
        SENSOR_INFO = 1, SENSOR_DATA, PLAN_IMAGE, PLAN_INFO
    };
    static public int command;
    static public string planId;
    static public void receivedData(byte[] data)
    {
        command = data[0];
        switch((EProtocol)command){
            case EProtocol.SENSOR_INFO:
                receiveSensorInfo(data);
                break;
            case EProtocol.SENSOR_DATA:
                receiveSensorData(data);
                break;
            case EProtocol.PLAN_IMAGE:
                receivePlanImage(data);
                break;
            case EProtocol.PLAN_INFO:
                receivePlanInfo(data);
                break;
            default:
                break;
        }
    }

    static public byte[] sendData(int command)
    {
        byte[] buf;
        switch ((EProtocol)command)
        {
            case EProtocol.SENSOR_INFO:
                buf = sendSensorInfo();
                break;
            case EProtocol.SENSOR_DATA:
                buf = sendSensorData();
                break;
            case EProtocol.PLAN_IMAGE:
                buf = sendPlanImage();
                break;
            case EProtocol.PLAN_INFO:
                buf = sendPlanInfo();
                break;
            default:
                buf = null;
                break;
        }
        return buf;
    }
    static public void receiveSensorInfo(byte[] data)
    {
        // 도면 ID 체크
    }

    static public void receiveSensorData(byte[] data)
    {

    }

    static public void receivePlanImage(byte[] data)
    {
        Console.WriteLine("Received plan image");

        byte[] temp = new byte[data.Length - 1];
        string buf;

        for(int i = 1; i < data.Length; i++){
            temp[i - 1] = data[i];
        }
        buf = Encoding.Default.GetString(temp);
        
        planId = buf;
        Console.WriteLine("Plan image id : " + buf);
    }
    static public void receivePlanInfo(byte[] data)
    {
        
    }

    static public byte[] sendSensorInfo()
    {
        Console.WriteLine("Send sensor info");

        List<byte> buf = new List<byte>();

        buf.Add(1);
        buf.Add(2);

        return buf.ToArray();
    }

    static public byte[] sendSensorData()
    {
        Console.WriteLine("Send sensor data");
        
        List<byte> buf = new List<byte>();

        buf.Add(3);
        buf.Add(4);

        return buf.ToArray();
    }
    static public byte[] sendPlanImage()
    {
        Console.WriteLine("Send plan image");

        List<byte> buf = new List<byte>();

        buf.Add(3);
        byte[] temp = RegistPlanModel.LoadPlanImage(planId);
        foreach(var v in temp){
            buf.Add(v);
        }

        return buf.ToArray();
    }
    static public byte[] sendPlanInfo()
    {
        Console.WriteLine("Send plan info");
        
        List<byte> buf = new List<byte>();

        buf = RegistPlanModel.LoadPlanInfoList();

        return buf.ToArray();
    }
}


