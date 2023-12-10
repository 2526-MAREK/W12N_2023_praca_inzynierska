using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using testingEnvironmentApp.Data;
using testingEnvironmentApp.Models;
using testingEnvironmentApp.Services.DataServices.Interfaces;

namespace testingEnvironmentApp.Services.DataServices
{
    public class MsrtPointDataService : IMsrtPointDataService
    {
        private readonly ApplicationDbContext _context;

        public MsrtPointDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddMsrtPoint(MsrtPoint newMsrtPoint)
        {
            if (_context == null)
            {
                Debug.WriteLine("Error: _context is null.");
                return;
            }

            // Sprawdzenie, czy urządzenie już istnieje
            var existingMsrtPoint = await _context.MsrtPoints
                                                .FirstOrDefaultAsync(d => d.MsrtPointIdentifier == newMsrtPoint.MsrtPointIdentifier);

            if (existingMsrtPoint != null)
            {
                Debug.WriteLine($"Error: Device with identifier {existingMsrtPoint.MsrtPointIdentifier} already exists.");
                return;
            }


            _context.MsrtPoints.Add(newMsrtPoint);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MsrtPoint>> GetAllMsrtPointFromDataBase()
        {
            return await _context.MsrtPoints.ToListAsync();
        }

        
    }
}
