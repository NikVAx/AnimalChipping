namespace Application.Abstractions.Interfaces
{
    public interface ICrudService<TEntity, TID>
    {
        public Task<TEntity?> GetByIdAsync(TID id);
        public Task<int> CreateAsync(TEntity entity);
        public Task<int> UpdateAsync(TEntity entity);
        public Task<int> DeleteAsync(TID id);
    }
}
