using testingEnvironmentApp.Models;

namespace testingEnvironmentApp.Services.DataServices.Interfaces
{
    public interface IMsrtPointDataService
    {
        public Task AddMsrtPoint(MsrtPoint newMsrtPoint);
        public Task<List<MsrtPoint>> GetAllMsrtPointFromDataBase();
    }
}
