namespace ApiMvc.Models
{
    public class Title
    {
        public int id { get; internal set; }
        public string title { get; set; }
        public int releaseYear { get; set; }
        public string gender { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public int ageRating { get; set; }
    }
}
