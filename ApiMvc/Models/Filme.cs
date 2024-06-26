namespace ApiMvc.Models
{
    public class Filme
    {
        public int id { get; internal set; }
        public string title { get; set; }
        public string description { get; set; }
        public string gender { get; set; }
        public DateOnly releaseDate { get; set; }
        public int classification {  get; set; }
        public int rate { get; set; }
    }
}
