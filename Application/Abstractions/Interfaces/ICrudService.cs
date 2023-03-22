namespace Application.Abstractions.Interfaces
{
    public interface ICrudService<TEntity, TKey>
    {
        public Task<TEntity?> GetByIdAsync(TKey id);
        public Task CreateAsync(TEntity entity);
        public Task UpdateAsync(TEntity entity);
        public Task DeleteAsync(TKey id);
    }
}
