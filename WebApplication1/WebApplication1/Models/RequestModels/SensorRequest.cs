namespace WebApplication1.Models.RequestModels
{
    public class SensorRequest
    {

        public string sensor_name { get; set; }

        public List<SensorMeasurementsRequest>? sensorMeasurements { get; set; }
    }
}
