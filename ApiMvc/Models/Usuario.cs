namespace ApiMvc.Models
{
    public class Usuario
    {
        public int id { get; internal set; }
        public string name { get; set; }
        public string cpf { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime birthday { get; set; }
    }
}
