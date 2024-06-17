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
    public class MeasuremeantTypeController : ControllerBase
    {
        private readonly SensorsContext _mtContext;
        public MeasuremeantTypeController(SensorsContext context)
        {
            _mtContext = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<MeasurementsType>>> GetMesType()
        {
            return await _mtContext.MeasurementsTypes.Select(measurements_type => measurements_type).ToListAsync();
        }
        [HttpGet(template: "{id}")]
        public async Task<ActionResult<MeasurementsType>> GetMesTypeItem(int id)
        {
            return await _mtContext.MeasurementsTypes.FindAsync(id) ?? throw new InvalidOperationException();
        }
        [HttpDelete(template: "{id}")]
        public async Task<ActionResult<List<MeasurementsType>>> DeleteMesType(int id)
        {
            MeasurementsType? mt = await _mtContext.MeasurementsTypes.FindAsync(id);
            List<SensorsMeasurement> ToDel = _mtContext.SensorsMeasurements.Where(e => e.type_id == id).ToList();
            if (mt != null && _mtContext.Measurements.Where(e => e.measuremnet_type == id).Count() == 0)
            {
                foreach (var SM in ToDel)
                {
                    _mtContext.SensorsMeasurements.Remove(SM);
                }
                _mtContext.MeasurementsTypes.Remove(mt);
            }
            await _mtContext.SaveChangesAsync();
            return NoContent();

        }
        [HttpPost]
        public async Task<ActionResult<List<MeasurementsType>>> AddMesType(MeasuremeantsTypeRequest mt)
        {
            _mtContext.MeasurementsTypes.Add(new MeasurementsType { type_name = mt.type_name, type_units = mt.type_units });
            await _mtContext.SaveChangesAsync();

            return NoContent();

        }
        [HttpPut]
        public async Task<ActionResult<List<MeasurementsType>>> UpdateMesType(int id, MeasuremeantsTypeRequest mt)
        {
            MeasurementsType need_mt = await _mtContext.MeasurementsTypes.FindAsync(id);
            if (need_mt != null)
            {
                need_mt.type_name = mt.type_name;
                need_mt.type_units = mt.type_units;
                await _mtContext.SaveChangesAsync();
            }
            return NoContent();
        }
    }
}
