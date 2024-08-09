using ApiMvc.Models;

namespace ApiMvc.Dtos
{
    public class TitleDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public int releaseYear { get; set; }
        public string gender { get; set; }
        public string image { get; set; }
        public string logo { get; set; }
        public string description { get; set; }
        public string detailedDescription { get; set; }
        public string type { get; set; }
        public int ageRating { get; set; }
    }
}
