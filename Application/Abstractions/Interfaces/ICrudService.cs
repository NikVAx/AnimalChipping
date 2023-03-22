namespace Application.Abstractions.Interfaces
{
    public interface ICrudService<TEntity, TID>
    {
        public Task<TEntity?> GetByIdAsync(TID id);
        public Task CreateAsync(TEntity entity);
        public Task UpdateAsync(TEntity entity);
        public Task DeleteAsync(TID id);
    }
}
