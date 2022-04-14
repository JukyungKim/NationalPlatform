
namespace NationalPlatform.Models;

class SensorList
{
    static private SensorList instance;
    static public SensorList sInstance{
        get{
            if(instance == null){
                instance = new SensorList();
            }
            return instance;
        }
    }

    public List<Sensor> sensor = new List<Sensor>();
}

class Sensor{
    public string id;
    public string address;
    public string building_name;
    public string dong;
    public string floor;
    public string ho;
    public string number;
    public string plan_id;
}