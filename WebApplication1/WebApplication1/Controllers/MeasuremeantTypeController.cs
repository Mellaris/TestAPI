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
    public class SensorController : ControllerBase
    {
        private readonly SensorsContext _sensorContext;

        public SensorController(SensorsContext context)
        {
            _sensorContext = context;
        }

        private static List<Sensor> sensors = new List<Sensor>();

        [HttpPut(template:"{id}")]
        public async Task<ActionResult<Sensor>> UpdateSensor(int id, Sensor sensor)
        {
            Sensor? findSensor = sensors.FindLast(it => it.sensor_id == id);
            if(findSensor != null) findSensor.sensor_name = sensor.sensor_name;
            return NoContent();
        }


        [HttpDelete(template: "{id}")]

        public async Task<ActionResult<Sensor?>> RemoveSensor(int id)
        {
            Sensor? removeSensor = sensors.FindLast(it => it.sensor_id == id);
            if (removeSensor != null) sensors.Remove(removeSensor);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Sensor>> AddSensor(SensorRequest sensor)
        {
            
          Sensor new_sensor = new Sensor { sensor_name = sensor.sensor_name };
            _sensorContext.Sensors.Add(new_sensor); 
            
            await _sensorContext.SaveChangesAsync();
            return new_sensor;
            
        }

        [HttpGet]
        public async Task<ActionResult<List<Sensor>>> GetSensors()
        {
            return await _sensorContext.Sensors.Select(sensor => sensor).ToListAsync();
        }

        [HttpGet(template: "{id}")]
        public async Task<ActionResult<Sensor>> GetSensorItem(int id)
        {
            return sensors
                .FindLast(it => it.sensor_id == id)
                ?? throw new InvalidOperationException();
        }


    }
}
