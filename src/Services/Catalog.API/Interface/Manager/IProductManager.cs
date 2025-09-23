using Catalog.API.Models;
using MongoRepo.Interfaces.Repository;

namespace Catalog.API.Interface.Manager
{
    public interface IProductManager:ICommonRepository<Product>
    {
       public List<Product> GetCategory(string category);
    }
}
