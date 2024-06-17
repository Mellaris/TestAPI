namespace WebApplication1.Models
{
    public class Measurement
    {
        public int? sensor_inventory_number { get; set; }

        public decimal? measurement_value { get; set; }

        public DateTime? measurement_ts { get; set; }

        public int? measuremnet_type { get; set; }

        public virtual MeasurementsType? MeasurementTypeNavigation { get; set; }

        public virtual MeteostationsSensor? SensorInventoryNumberNavigation { get; set; }
    }
}
