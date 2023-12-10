using testingEnvironmentApp.Models;

namespace testingEnvironmentApp.Services.DataServices.Interfaces
{
    public interface IMsrtAssociationDataService
    {
        public Task AddMsrtAssociation(string msrtPointIdentifier, string hubIdentifier, string additionalInfo);
        public Task<string> GetMsrtPointIdentifierWithChannelIdentifier(string channelIdentifier);
        public Task<List<MsrtAssociation>> GetAllMstrAssociation();
    }
}
