using System.Data.SqlTypes;
using System.Text.Json.Serialization;

namespace ApiMvc.Models
{
    public class Subscription
    {
        public int id { get; internal set; }
        public string name { get; set; }
        public float value { get; set; }
        public string period { get; set; }
        [JsonIgnore]
        public ICollection<Usuario> Usuarios { get; set; }
    }
}
