namespace WebApplication1.Models.ResposneModels
{
    public class MeteostationsSensorsResponseForMeteostations
    {
        public int sensor_inventory_number { get; set; }

        public DateTime? added_ts { get; set; }

        public DateTime? removed_ts { get; set; }
        public int sensor_id { get; set; }
        public string sensor_name { get; set; }
    }
}
