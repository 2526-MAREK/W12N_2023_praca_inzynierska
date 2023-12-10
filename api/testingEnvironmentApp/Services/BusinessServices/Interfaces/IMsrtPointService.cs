using testingEnvironmentApp.Models;

namespace testingEnvironmentApp.Services.BusinessServices.Interfaces
{
    public interface IMsrtPointService
    {
        public Task CreateNewMsrtPoint(string msrtPointIdentifier, string name, string description, string factor, string location);
        public Task<List<MsrtPoint>> GetAllMstrPointsFromDataBase();
    }
}
