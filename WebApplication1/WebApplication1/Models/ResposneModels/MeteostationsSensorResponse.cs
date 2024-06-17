namespace WebApplication1.Models.ResposneModels
{
    public class MeteostationsSensorResponse
    {
        public int station_id { get; set; }
        public string? station_name { get; set; }

        public decimal? station_longitude { get; set; }

        public decimal? station_latitude { get; set; }
        public List<SensorResponseForMeteostationsSensor> sensors { get; set; }
    }
}
