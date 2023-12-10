using testingEnvironmentApp.Models;
using testingEnvironmentApp.Services.BusinessServices.Interfaces;
using testingEnvironmentApp.Services.DataServices;
using testingEnvironmentApp.Services.DataServices.Interfaces;

namespace testingEnvironmentApp.Services.BusinessServices
{
    public class MsrtAssociationService : IMsrtAssociationService
    {
        private readonly IMsrtAssociationDataService _msrtAssociationDataService;

        public MsrtAssociationService(IMsrtAssociationDataService msrtAssociationDataService)
        {
            _msrtAssociationDataService = msrtAssociationDataService;
        }

        public async Task CreateNewMsrtPoint(string msrtPointIdentifier, string hubIdentifier, string additionalInfo)
        {
           
            await _msrtAssociationDataService.AddMsrtAssociation(msrtPointIdentifier, hubIdentifier, additionalInfo);
        }

        public async Task<string> GetMsrtPointIdentifierWithChannelIdentifierFromDataBase(string channelIdentifier)
        {
            return await _msrtAssociationDataService.GetMsrtPointIdentifierWithChannelIdentifier(channelIdentifier);
        }

        public async Task<List<MsrtAssociation>> GetAllMstrAssociationFromDataBase()
        {
            return await _msrtAssociationDataService.GetAllMstrAssociation();
        }
    }
}
