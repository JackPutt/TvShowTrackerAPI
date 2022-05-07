using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TvShowTrackerApi.Data;
using TvShowTrackerApi.Models;

namespace TvShowTrackerApi.Controllers
{
    [ApiController]
    public class EpisodeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EpisodeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Get all episodes from within a given users series
        [Route("api/episodes/{seriesId}/{userId}")]
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<Episode>> Index(string seriesId, string userId)
        {
            return await _context.Episodes.Where(x => x.SeriesID == seriesId && x.UserID == userId).OrderBy(x => x.SeasonNumber).ThenBy(x => x.EpisodeNumber).ToArrayAsync();
        }

        //POST: Add all epsiodes for a given series 
        [Route("api/episode")]
        [HttpPost]
        [Authorize]
        public async Task<Episode> Create([Bind("EpisodeID,SeriesID,UserID,SeasonNumber,EpisodeTitle,EpisodeImage,Watched,EpisodeNumber")] Episode episodes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(episodes);
                await _context.SaveChangesAsync();
                return episodes;
            }
            return episodes;
        }
        //POST: Update episodes to flag when they have been watched
        [Route("api/watchedepisode")]
        [HttpPost]
        [Authorize]
        public async Task<Episode> WatchedEpisode([Bind("EpisodeID,SeriesID,UserID,SeasonNumber,EpisodeTitle,EpisodeImage,Watched,EpisodeNumber")] Episode episodes)
        {
            if (ModelState.IsValid)
            {
                var episode = await _context.Episodes.Where(x => x.SeriesID == episodes.SeriesID && x.UserID == episodes.UserID && x.EpisodeID == episodes.EpisodeID).FirstOrDefaultAsync();
                _context.Episodes.Remove(episode);
                _context.Add(episodes);
                await _context.SaveChangesAsync();
                return episodes;
            }
            return episodes;
        }
        //DELETE: Remove a epsiode 
        [Route("api/episode")]
        [HttpDelete]
        [Authorize]
        public async Task<Episode> DeleteConfirmed(string id)
        {
            var episodes = await _context.Episodes.FindAsync(id);
            _context.Episodes.Remove(episodes);
            await _context.SaveChangesAsync();
            return episodes;
        }

        private bool EpisodesExists(string id)
        {
            return _context.Episodes.Any(e => e.EpisodeID == id);
        }
    }
}
