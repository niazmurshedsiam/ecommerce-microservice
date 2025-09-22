using Catalog.API.Models;
using MongoRepo.Interfaces.Manager;

namespace Catalog.API.Interface.Repository
{
    public interface IProductRepository:ICommonManager<Product>
    {
    }
}
