namespace WebApplication1.Models
{
    public class Meteostation
    {
        public int station_id { get; set; }

        public string? station_name { get; set; }

        public decimal? station_longitude { get; set; }

        public decimal? station_latitude { get; set; }

        public virtual ICollection<MeteostationsSensor> MeteostationsSensors { get; set; } = new List<MeteostationsSensor>();
    }
}
