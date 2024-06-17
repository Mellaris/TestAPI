namespace WebApplication1.Models
{
    public class MeteostationsSensor
    {
        public int sensor_inventory_number { get; set; }

        public int? station_id { get; set; }

        public int? sensor_id { get; set; }

        public DateTime? added_ts { get; set; }

        public DateTime? removed_ts { get; set; }

        public virtual Sensor? Sensor { get; set; }

        public virtual Meteostation? Station { get; set; }
    }
}
