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
    public class MeteostationController : ControllerBase
    {
        private readonly SensorsContext _meteostationsContext;
        public MeteostationController(SensorsContext context)
        {
            _meteostationsContext = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<MeteostationResponse>>> GetMeteostations()
        {

            var meteostations = await _meteostationsContext.Meteostations
                .Select(m => new MeteostationResponse { station_id = m.station_id, station_name = m.station_name, station_latitude = m.station_latitude, station_longitude = m.station_longitude }).ToListAsync();
            foreach (var meteostation in meteostations)
            {
                meteostation.meteostationsSensors = await _meteostationsContext.MeteostationsSensors.Where(mes => mes.station_id == meteostation.station_id).Join(
                    _meteostationsContext.Sensors,
                    m => m.sensor_id,
                    s => s.sensor_id,
                    (m, s) => new MeteostationsSensorsResponseForMeteostations
                    {
                        sensor_inventory_number = m.sensor_inventory_number,
                        sensor_id = s.sensor_id,
                        sensor_name = s.sensor_name,
                        added_ts = m.added_ts,
                        removed_ts = m.removed_ts
                    }
                    ).ToListAsync();
            }
            return meteostations;
        }
        [HttpGet(template: "{id}")]
        public async Task<ActionResult<MeteostationResponse>> GetMeteostationsItem(int id)
        {
            Meteostation ms = _meteostationsContext.Meteostations.Find(id);
            var meteostation = new MeteostationResponse { station_id = ms.station_id, station_name = ms.station_name, station_latitude = ms.station_latitude, station_longitude = ms.station_longitude };
            meteostation.meteostationsSensors = await _meteostationsContext.MeteostationsSensors.Where(mes => mes.station_id == meteostation.station_id).Join(
                _meteostationsContext.Sensors,
                m => m.sensor_id,
                s => s.sensor_id,
                (m, s) => new MeteostationsSensorsResponseForMeteostations
                {
                    sensor_inventory_number = m.sensor_inventory_number,
                    sensor_id = s.sensor_id,
                    sensor_name = s.sensor_name,
                    added_ts = m.added_ts,
                    removed_ts = m.removed_ts
                }
                ).ToListAsync();
            return meteostation ?? throw new InvalidOperationException();
        }
        [HttpDelete(template: "{id}")]
        public async Task<ActionResult<List<Meteostation>>> DeleteMeteostation(int id)
        {
            Meteostation? meteostation = await _meteostationsContext.Meteostations.FindAsync(id);
            List<int> sync = _meteostationsContext.MeteostationsSensors.Where(e => e.station_id == id).Select(s => s.sensor_inventory_number).ToList();
            bool Go = true;
            foreach (var invnum in sync)
            {
                if (_meteostationsContext.Measurements
                        .Where(e => e.sensor_inventory_number.ToString().Contains(invnum.ToString())).Count() != 0)
                {
                    Go = false;
                }
            }

            if (Go && meteostation != null)
            {
                _meteostationsContext.Meteostations.Remove(meteostation);
                foreach (var invnum in sync)
                {
                    _meteostationsContext.MeteostationsSensors.Remove(
                        _meteostationsContext.MeteostationsSensors.FirstOrDefault(
                            e => e.sensor_inventory_number == invnum));
                }
            }
            await _meteostationsContext.SaveChangesAsync();
            return NoContent();

        }
        [HttpPost]
        public async Task<ActionResult<List<Meteostation>>> AddMeteostation(MeteostationRequest meteostation)
        {
            _meteostationsContext.Meteostations.Add(new Meteostation() { station_name = meteostation.station_name, station_latitude = meteostation.station_latitude, station_longitude = meteostation.station_longitude });
            await _meteostationsContext.SaveChangesAsync();
            return NoContent();

        }
        [HttpPut]
        public async Task<ActionResult<List<Meteostation>>> UpdateMeteostation(int id, MeteostationRequest meteostation)
        {
            Meteostation need_meteostation = await _meteostationsContext.Meteostations.FindAsync(id);
            if (need_meteostation != null)
            {
                need_meteostation.station_name = meteostation.station_name;
                need_meteostation.station_latitude = meteostation.station_latitude;
                need_meteostation.station_longitude = meteostation.station_longitude;
                await _meteostationsContext.SaveChangesAsync();
            }
            return NoContent();
        }
    }
}
