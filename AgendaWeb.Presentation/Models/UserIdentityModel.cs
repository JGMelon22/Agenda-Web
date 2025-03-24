namespace AgendaWeb.Presentation.Models
{
    public class UserIdentityModel
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataHoraAcesso { get; set; }
    }
}
