namespace ApiMvc.Models
{
    public class Episode
    {
        public int id { get; internal set; }
        public string title { get; set; }
        public string description { get; set; }
        public float duration { get; set; }
        public int number { get; set; }
        public int idSeason { get; set; }
    }
}
