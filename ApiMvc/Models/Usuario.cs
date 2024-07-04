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
        public Subscription subscription { get; set; }
    }
}
