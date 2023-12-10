using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Channels;
using testingEnvironmentApp.Data;
using testingEnvironmentApp.Models;
using testingEnvironmentApp.Models.Devices;
using testingEnvironmentApp.Services.DataServices.Interfaces;

namespace testingEnvironmentApp.Services.DataServices
{
    public class MsrtAssociationDataService : IMsrtAssociationDataService
    {
        private readonly ApplicationDbContext _context;

        public MsrtAssociationDataService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddMsrtAssociation(string msrtPointIdentifier,string hubIdentifier, string additionalInfo)
        {
            if (_context == null)
            {
                Debug.WriteLine("Error: _context is null.");
                return;
            }

            var idPoint = await _context.MsrtPoints
            .Where(d => d.MsrtPointIdentifier == msrtPointIdentifier)
            .Select(d => d.IdPoint)
            .FirstOrDefaultAsync();

            var idHub = await _context.Hubs
                .Where(d => d.HubIdentifier == hubIdentifier)
            .Select(d => d.IdHub)
            .FirstOrDefaultAsync();

            var deviceIds = _context.Devices
                                .Where(device => device.IdHub == idHub)
                                .Select(device => device.IdDevice)
                                .ToList();



            var channelIdentifiers = _context.Channels
            .Where(dc => deviceIds.Contains(dc.IdDevice))
            .Select(dc => dc.ChannelIdentifier)
            .ToList();


            foreach (var channelIdentifier in channelIdentifiers)
            {
                

                var channel = await _context.Channels
                    .FirstOrDefaultAsync(c => c.ChannelIdentifier == channelIdentifier);

                
                if (channel == null)
                {
                    throw new ArgumentException("Channel not found");
                }
                var idDevice = channel.IdDevice;


                var msrt = await _context.Msrts.FirstOrDefaultAsync(c => (c.IdChannels == channel.IdChannel && c.IdHubs == idHub && c.IdDevice == idDevice));

                var existingMsrtAssociation = await _context.MsrtAssociations
    .FirstOrDefaultAsync(c => (c.IdChannels == channel.IdChannel && c.IdHubs == idHub && c.IdDevice == idDevice));

                if (existingMsrtAssociation == null)
                {
                    var msrtAssociation = new MsrtAssociation
                    {
                        IdPoint = idPoint,
                        IdHubs = idHub,
                        IdDevice = msrt.IdDevice,
                        IdChannels = msrt.IdChannels,
                        DataTimeMs = msrt.DataTimeMs,
                        DateTimeZone = msrt.DateTimeZone,
                        AdditionalInfo = "Additional Info"
                    };


                    _context.MsrtAssociations.Add(msrtAssociation);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    Debug.WriteLine("This msrtAssociation is exists");
                }
            }


        }


        public async Task<string> GetMsrtPointIdentifierWithChannelIdentifier(string channelIdentifier)
        {
            var idChannel = await _context.Channels
          .Where(d => d.ChannelIdentifier == channelIdentifier)
          .Select(d => d.IdChannel)
          .FirstOrDefaultAsync();

            var idPoint = await _context.MsrtAssociations
         .Where(d => d.IdChannels == idChannel)
         .Select(d => d.IdPoint)
         .FirstOrDefaultAsync();


            var msrtPointIdentifier = await _context.MsrtPoints
        .Where(d => d.IdPoint == idPoint)
        .Select(d => d.MsrtPointIdentifier)
        .FirstOrDefaultAsync();

            if(msrtPointIdentifier != null)
            {
                return msrtPointIdentifier.ToString();
            }
            else
            {
                return null;
            }
        }

        public async Task<List<MsrtAssociation>> GetAllMstrAssociation()
        {
            return await _context.MsrtAssociations .ToListAsync();
        }
    }
}
