using ApiMvc.Models;

namespace ApiMvc.Dtos
{
    public class EpisodeDTO
    {
        public int id { get; internal set; }
        public string title { get; set; }
        public string description { get; set; }
        public float duration { get; set; }
        public string image { get; set; }
        public int number { get; set; }
        public int idSeason { get; set; }
    }
}
