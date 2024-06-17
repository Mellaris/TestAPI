namespace WebApplication1.Models.RequestModels
{
    public class MeteostationRequest
    {
        public string? station_name { get; set; }

        public decimal? station_longitude { get; set; }

        public decimal? station_latitude { get; set; }
    }
}
