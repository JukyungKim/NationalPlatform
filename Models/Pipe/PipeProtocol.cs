
using System.Text;
using NationalPlatform.Models;

public class PipeProtocol{
    enum EProtocol
    {
        SENSOR_INFO = 1, SENSOR_DATA, PLAN_IMAGE, PLAN_INFO
    };
    static public int command;
    static public string planId;
    static public List<string> sensorIdList;
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
        // string planId;
        byte[] temp = new byte[data.Length - 1];
        List<string> sensorIdList = new List<string>();

        for(int i = 1; i < data.Length; i++){
            temp[i - 1] = data[i];
        }
        planId = Encoding.Default.GetString(temp);
        Console.WriteLine("Plan image id : " + planId);
        RegistSensorModel.SearchSensor(planId, sensorIdList);

        foreach(var s in sensorIdList){
            Console.WriteLine("Sensor id list : " + s);
        }

        PipeProtocol.sensorIdList = sensorIdList;
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



        return buf.ToArray();
    }

    static public byte[] sendSensorData()
    {
        Console.WriteLine("Send sensor data");
        
        List<byte> buf = new List<byte>();

        buf.Add(2);

        List<SensorData> sensorListInPlan = new List<SensorData>();

        // 도면에 해당하는 센서들 검색
        foreach(var idDb in sensorIdList){  // sensorIdList : 도면안에 있는 센서리스트(DB 검색)
            foreach(var sensorTcp in ReceiveSensorData.sensorList){ // sensorList : 수신 센서
                if(idDb == sensorTcp.id.ToString()){
                    sensorListInPlan.Add(sensorTcp);        // sensorListInPlan : 도면 안에 있고 수신 센서
                    Console.WriteLine("Tcp sensor : " + idDb);
                }
            }
        }
        
        // 센서 데이터를 바이트 배열로 변환
        List<byte[]> temp = new List<byte[]>();
        
        // 6byte:id, 4byte:smoke,temp,gas
        foreach(var s in sensorListInPlan){
            temp.Add(ConvertToByte(s.id).Take(6).ToArray());
            temp.Add(ConvertToByte(s.smoke));
            temp.Add(ConvertToByte(s.temp));
            temp.Add(ConvertToByte(s.gas));
        }

        foreach(var t in temp){
            foreach(var b in t){
                buf.Add(b);
            }
        }
        Console.WriteLine("Send sensor data buf : " + buf.Count);

        return buf.ToArray();
    }

    static public byte[] ConvertToByte(int t)
    {
        return BitConverter.GetBytes(t);
    }
    static public byte[] ConvertToByte(ulong t)
    {
        return BitConverter.GetBytes(t);
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


