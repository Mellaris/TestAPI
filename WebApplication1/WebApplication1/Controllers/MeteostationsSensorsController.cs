using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.RequestModels;
using WebApplication1.Models.ResposneModels;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeteostationsSensorsController : ControllerBase
    {
        private readonly SensorsContext _msContext;
        public MeteostationsSensorsController(SensorsContext context)
        {
            _msContext = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<MeteostationsSensorResponse>>> GetConnections()
        {
            var Connections = await _msContext.Meteostations.Select(s => new MeteostationsSensorResponse
            {
                station_id = s.station_id,
                station_name = s.station_name,
                station_latitude = s.station_latitude,
                station_longitude = s.station_longitude
            }).ToListAsync();
            foreach (var Connection in Connections)
            {
                Connection.sensors = await _msContext.MeteostationsSensors.Where(e => e.station_id == Connection.station_id)
                    .Join(_msContext.Sensors,
                    ms => ms.sensor_id,
                    s => s.sensor_id,
                    (ms, s) => new SensorResponseForMeteostationsSensor
                    {
                        sensor_id = s.sensor_id,
                        sensor_name = s.sensor_name,
                        added_ts = ms.added_ts,
                        sensor_inventory_number = ms.sensor_inventory_number,
                        removed_ts = ms.removed_ts,
                    }
                    ).ToListAsync();
            }
            return Connections;
        }
        [HttpPost]
        public async Task<ActionResult<List<MeteostationsSensor>>> AddConnection(List<MSRequest> meteostations_sensors)
        {
            foreach (MSRequest ms in meteostations_sensors)
            {
                if (ms.added_ts == null)
                {
                    ms.added_ts = DateTime.UtcNow;
                }
                _msContext.MeteostationsSensors.Add(new MeteostationsSensor { sensor_id = ms.sensor_id, station_id = ms.station_id, added_ts = ms.added_ts });
            }
            await _msContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult<List<MeteostationsSensor>>> DeleteSensor(int id, DateTime removed_ts)
        {

            MeteostationsSensor ms = await _msContext.MeteostationsSensors.FindAsync(id);
            if (removed_ts == null)
            {
                removed_ts = DateTime.UtcNow;
            }
            ms.removed_ts = removed_ts;
            await _msContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
