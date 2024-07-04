using System.Text.Json.Serialization;

namespace ApiMvc.Models
{
    public class Profile
    {
        public int id { get; internal set; }
        public string name { get; set; }
        public string type { get; set; }
        public string image { get; set; }
        public int idUser { get; set; }
        [JsonIgnore]
        public Usuario usuario { get; set; }
    }
}
