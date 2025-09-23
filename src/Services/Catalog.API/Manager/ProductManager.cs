using Catalog.API.Interface.Manager;
using Catalog.API.Models;
using Catalog.API.Repository;
using MongoRepo.Manager;
using MongoRepo.Repository;

namespace Catalog.API.Manager
{
    public class ProductManager : CommonManager<Product>, IProductManager
    {
        public ProductManager() : base(new ProductRepository())
        {
        }

        public List<Product> GetCategory(string category)
        {
            return GetAll(c=> c.Category == category).ToList();
        }
    }
}
