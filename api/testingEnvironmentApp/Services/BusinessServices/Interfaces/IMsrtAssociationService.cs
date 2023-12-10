using testingEnvironmentApp.Models;

namespace testingEnvironmentApp.Services.BusinessServices.Interfaces
{
    public interface IMsrtAssociationService
    {
        public Task CreateNewMsrtPoint(string msrtPointIdentifier, string hubIdentifier, string additionalInfo);
        public Task<string> GetMsrtPointIdentifierWithChannelIdentifierFromDataBase(string channelIdentifier);
        public Task<List<MsrtAssociation>> GetAllMstrAssociationFromDataBase();
    }
}
