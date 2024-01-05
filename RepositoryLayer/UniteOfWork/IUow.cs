using DomainLayer.Entities;
using RepositoryLayer.Repositories;

namespace RepositoryLayer.UniteOfWork
{
    public interface IUow
    {
        IRepository<T> GetRepository<T>() where T : BaseEntity;
        Task SaveChangesAsync();
    }
}