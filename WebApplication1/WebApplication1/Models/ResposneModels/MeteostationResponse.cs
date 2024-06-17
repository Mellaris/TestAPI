namespace WebApplication1.Models.ResposneModels
{
    public class MeteostationResponse
    {
        public int station_id { get; set; }

        public string? station_name { get; set; }

        public decimal? station_longitude { get; set; }

        public decimal? station_latitude { get; set; }
        public List<MeteostationsSensorsResponseForMeteostations> meteostationsSensors { get; set; } = null;
    }
}
