using System.Numerics;
using testingEnvironmentApp.Models;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;
using testingEnvironmentApp.Services.DataServices.Interfaces;

namespace testingEnvironmentApp.Services.BusinessServices
{
    public class MsrtPointService : IMsrtPointService
    {
        private readonly IMsrtPointDataService _msrtPointDataService;

        public MsrtPointService(IMsrtPointDataService msrtPointDataService)
        {
            _msrtPointDataService = msrtPointDataService;
        }

        public async Task CreateNewMsrtPoint(string msrtPointIdentifier, string name , string description, string factor, string location)
        {
            var msrtPoint = new MsrtPoint { MsrtPointIdentifier = msrtPointIdentifier, Name = name, Description = description, Factor = factor, Location = location };
            await _msrtPointDataService.AddMsrtPoint(msrtPoint);
        }

        public async Task<List<MsrtPoint>> GetAllMstrPointsFromDataBase()
        {
            return await _msrtPointDataService.GetAllMsrtPointFromDataBase();
        }
    }
}
