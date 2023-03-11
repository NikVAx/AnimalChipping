namespace Application.Abstractions.Interfaces
{
    public interface ICrudService<TEntity, TID>
    {
        public Task<TEntity?> GetByIdAsync(TID id);
        public Task<int> AddAsync(TEntity entity);
        public Task<int> UpdateAsync(TEntity entity);
        public Task<int> RemoveAsync(TID id);
    }
}
