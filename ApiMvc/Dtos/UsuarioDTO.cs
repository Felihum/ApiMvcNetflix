namespace ApiMvc.Dtos
{
    public class UsuarioDTO
    {
        public int id { get; set; }
        public string cpf { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime birthday { get; set; }
        public int? idSubscription { get; set; }
    }
}
