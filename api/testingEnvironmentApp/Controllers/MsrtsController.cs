using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using testingEnvironmentApp.Models;
using testingEnvironmentApp.Models.Alarms;
using testingEnvironmentApp.Services.BusinessServices;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;

namespace testingEnvironmentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MsrtsController : Controller
    {
        IMsrtService _msrtService;
        public MsrtsController(IMsrtService msrtService)
        {
            _msrtService = msrtService;
        }


        ///http://localhost:5193/api/Msrts/tempSensor_1?startDate=2023-11-23T12%3A00%3A00&endDate=2023-11-23T12%3A59%3A59.999
        [HttpGet("{channelIdentifier}")]
        public async Task<ActionResult<IEnumerable<Msrt>>> GetMstrFromDataBaseWithDataRangeAndChannelIdentifier(
    string channelIdentifier,
    [FromQuery] DateTime startDate,
    [FromQuery] DateTime endDate)
        {
            Debug.WriteLine(channelIdentifier);
            Debug.WriteLine(startDate);
            Debug.WriteLine(endDate);
            return await _msrtService.GetMeasurementsByChannelAndDateRangeFromDataBase(channelIdentifier, startDate, endDate);
        }

    }
}
