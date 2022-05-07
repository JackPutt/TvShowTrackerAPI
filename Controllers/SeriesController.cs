using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TvShowTrackerApi.Models;
using TvShowTrackerApi.Data;

namespace TvShowTrackerApi.Controllers
{
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SeriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: All series that have been added to the users watch list
        [Route("api/series/{userId}")]
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<Series>> Get(string userId)
        {
            var series = await _context.Series.Where(m => m.UserID == userId).ToArrayAsync();
            return series;
        }

        //POST: Add a series to a users watch list
        [Route("api/series")]
        [HttpPost]
        [Authorize]
        public async Task<Series> Create([Bind("SeriesID,UserID,SeriesTitle,SeriesDescription,SeriesImage")] Series series)
        {
            if (ModelState.IsValid)
            {
                _context.Add(series);
                await _context.SaveChangesAsync();
                return series;
            }
            return series;
        }

        //DELETE: Remove a series from a users watch lsit
        [Route("api/series/{userId}/{id}")]
        [HttpDelete]
        [Authorize]
        public async Task<Series> DeleteConfirmed(string userId, string id)
        {
            var series = await _context.Series.Where(m => m.UserID == userId && m.SeriesID == id).FirstOrDefaultAsync();
            _context.Series.Remove(series);

            await _context.SaveChangesAsync();
            return series;
        }

        private bool SeriesExists(string id)
        {
            return _context.Series.Any(e => e.SeriesID == id);
        }
    }
}
