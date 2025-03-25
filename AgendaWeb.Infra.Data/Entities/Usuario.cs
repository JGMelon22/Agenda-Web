namespace AgendaWeb.Infra.Data.Entities
{
    /// <summary>
    /// Classe de entidade para usuário
    /// </summary>
    public class Usuario
    {
        public int GUID { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public DateTime DataInclusao { get; set; }
        public Guid Id { get; set; }
    }
}
