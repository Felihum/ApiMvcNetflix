using System.Text.Json.Serialization;

namespace ApiMvc.Models
{
    public class Usuario
    {
        public int id { get; internal set; }
        public string cpf { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime birthday { get; set; }
        public int idSubscription { get; set; }
        [JsonIgnore]
        public Subscription subscription { get; set; }
        [JsonIgnore]
        public ICollection<Profile> profiles { get; set; }
    }
}
