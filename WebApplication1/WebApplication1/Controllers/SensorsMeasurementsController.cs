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
    public class SensorsMeasurementsController : ControllerBase
    {
        private readonly SensorsContext _smContext;
        public SensorsMeasurementsController(SensorsContext context)
        {
            _smContext = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<SensorsMeasurement>>> GetSensorMeasurement()
        {
            return await _smContext.SensorsMeasurements.Select(SensorsMeasurement => SensorsMeasurement).ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<List<SensorsMeasurement>>> AddSensorMeasurement(SensorMeasurementsRequest SensorMeasurement, int sensor_id)
        {
            SensorsMeasurement new_sm = new SensorsMeasurement { sensor_id = sensor_id, type_id = SensorMeasurement.type_id, measurment_formula = SensorMeasurement.measurement_formula };
            _smContext.SensorsMeasurements.Add(new_sm);
            await _smContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult<List<SensorsMeasurement>>> DeleteSensorMeasurement(int? sensor_id, int? measurement_type)
        {
            if (sensor_id != null)
            {
                _smContext.SensorsMeasurements.RemoveRange(_smContext.SensorsMeasurements.Where(sm => sm.sensor_id == sensor_id));
            }
            else if (sensor_id != null)
            {
                _smContext.SensorsMeasurements.RemoveRange(_smContext.SensorsMeasurements.Where(sm => sm.sensor_id == sensor_id));
            }
            await _smContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
