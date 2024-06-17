namespace WebApplication1.Models
{
    public class SensorsMeasurement
    {
        public int? sensor_id { get; set; }

        public int? type_id { get; set; }

        public string? measurment_formula { get; set; }

        public virtual Sensor? Sensor { get; set; }

        public virtual MeasurementsType? Type { get; set; }
    }
}
