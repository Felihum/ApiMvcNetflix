using ApiMvc.Models;
using System.Text.Json.Serialization;

namespace ApiMvc.Dtos
{
    public class SubscriptionDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public float value { get; set; }
        public string period { get; set; }
    }
}
