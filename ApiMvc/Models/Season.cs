using System.Text.Json.Serialization;

namespace ApiMvc.Models
{
    public class Season
    {
        public int id { get; internal set; }
        public int number { get; set; }
        public int idTitle { get; set; }
        [JsonIgnore]
        public Title title { get; set; }
        public ICollection<Episode> episodes { get; set; }
    }
}
