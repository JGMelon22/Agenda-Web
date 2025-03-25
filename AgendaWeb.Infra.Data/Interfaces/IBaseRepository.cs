namespace AgendaWeb.Infra.Data.Interfaces
{
    /// <summary>
    /// Interface genérica para as operações do repositorio
    /// </summary>
    public interface IBaseRepository<TEntity>
        where TEntity : class
    {
        void Create(TEntity obj);
        void Update(TEntity obj);
        void Delete(TEntity obj);

        List<TEntity> GetAll();
        TEntity? GetById(Guid id);
    }
}
