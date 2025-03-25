using AgendaWeb.Infra.Data.Entities;

namespace AgendaWeb.Infra.Data.Interfaces
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        /// <summary>
        /// Método para retornar os dados de 1 Usuário baseado no email
        /// </summary>
        /// <param name="email">Email do usuário</param>
        /// <returns>Objeto Usuario ou null se não for encontrado</returns>
        Usuario? GetByEmail(string email);

        /// <summary>
        /// Método para retornar os dados de 1 Usuário baseado no email e na senha
        /// </summary>
        /// <param name="email">Email do usuário</param>
        /// <param name="senha">Senha do usuário</param>
        /// <returns>Objeto Usuario ou null se não for encontrado</returns>
        Usuario? GetByEmailESenha(string email, string senha);

    }
}
