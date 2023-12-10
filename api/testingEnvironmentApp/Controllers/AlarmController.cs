using Microsoft.AspNetCore.Mvc;
using testingEnvironmentApp.Models.Alarms;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;

namespace testingEnvironmentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmController : Controller
    {
        private readonly IAlarmService _alarmService;

        public AlarmController(IAlarmService alarmService)
        {
            _alarmService = alarmService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alarm>>> GetAlarms()
        {
            return await _alarmService.GetAllAlarmsFromDataBase();
        }
    }
}
