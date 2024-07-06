using ApiMvc.Models;
using System.Text.Json.Serialization;

namespace ApiMvc.Dtos
{
    public class SeasonDTO
    {
        public int id { get; set; }
        public int number { get; set; }
        public int idTitle { get; set; }
    }
}
