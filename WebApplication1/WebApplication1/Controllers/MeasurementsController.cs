using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.RequestModels;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private readonly SensorsContext _measurementContext;
        public MeasurementsController(SensorsContext context)
        {
            _measurementContext = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Measurement>>> GetMeasurements()
        {
            return await _measurementContext.Measurements.ToListAsync();
        }
        [HttpGet("/GetMeasurementsWithValues")]
        public async Task<ActionResult<List<Measurement>>> GetMeasurementsWithValues(int? meteostation_id, int? inventory_num)
        {
            List<Measurement> answer = new List<Measurement>();
            if (meteostation_id != null)
            {
                var MetsSens = _measurementContext.MeteostationsSensors.Where(mt => mt.station_id == meteostation_id).ToList();
                var ids = _measurementContext.Measurements.Select(mt => mt.sensor_inventory_number).ToList();
                foreach (var MetSen in MetsSens)
                {
                    if (ids.Contains(MetSen.sensor_inventory_number))
                    {
                        List<Measurement> tmp = _measurementContext.Measurements
                            .Where(m => m.sensor_inventory_number == MetSen.sensor_inventory_number).ToList();
                        answer.AddRange(tmp);
                    }
                }
            }
            else if (inventory_num != null)
            {
                answer = _measurementContext.Measurements.Where(m => m.sensor_inventory_number == inventory_num).ToList();
            }
            return answer;
        }
        [HttpPost]
        public async Task<ActionResult<List<Measurement>>> AddConnection(List<MeasuremeantRequest> measurements)
        {
            foreach (var measuremeant in measurements)
            {
                _measurementContext.Measurements.Add(new Measurement
                {
                    measurement_ts = measuremeant.measurement_ts,
                    measuremnet_type = measuremeant.measurement_type,
                    measurement_value = measuremeant.measurement_value,
                    sensor_inventory_number = measuremeant.sensor_inventory_number,
                });
            }
            await _measurementContext.SaveChangesAsync();
            return NoContent();

        }
        [HttpDelete]
        public async Task<ActionResult<List<Measurement>>> DeleteConnection(int? Inventory_number, int? meteostation_id)
        {

            if (Inventory_number != null)
            {
                _measurementContext.Measurements.RemoveRange(_measurementContext.Measurements.Where(m => m.sensor_inventory_number == Inventory_number));
            }
            else if (meteostation_id != null)
            {
                var MetsSens = _measurementContext.MeteostationsSensors.Where(mt => mt.station_id == meteostation_id).ToList();
                var ids = _measurementContext.Measurements.Select(mt => mt.sensor_inventory_number).ToList();
                foreach (var MetSen in MetsSens)
                {
                    if (ids.Contains(MetSen.sensor_inventory_number))
                    {
                        List<Measurement> tmp = _measurementContext.Measurements
                            .Where(m => m.sensor_inventory_number == MetSen.sensor_inventory_number).ToList();
                        _measurementContext.Measurements.RemoveRange(tmp);
                    }
                }
            }
            await _measurementContext.SaveChangesAsync();
            return NoContent();

        }
    }
}
