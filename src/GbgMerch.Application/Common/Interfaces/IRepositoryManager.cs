using GbgMerch.Domain.Interfaces;

namespace GbgMerch.Application.Common.Interfaces;

public interface IRepositoryManager
{
    IProductRepository ProductRepository { get; }
    IUnitOfWork UnitOfWork { get; }
}
