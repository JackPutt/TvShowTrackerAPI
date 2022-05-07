using System.ComponentModel.DataAnnotations;
namespace TvShowTrackerApi.Models
{
    public class Series
    {
        [Key]
        public string SeriesID { get; set; }
        public string UserID { get; set; }
        public string SeriesTitle { get; set; }
        public string SeriesDescription { get; set; }
        public string SeriesImage { get; set; }
    }
}
