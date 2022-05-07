using System.ComponentModel.DataAnnotations;
namespace TvShowTrackerApi.Models
{
    public class Episode
    {
        [Key]
        public string EpisodeID { get; set; }
        public string SeriesID { get; set; }
        public string UserID { get; set; }
        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }
        public string EpisodeTitle { get; set; }
        public string EpisodeImage { get; set; }
        public bool Watched { get; set; }
    }
}
