using Catalog.API.Interface.Manager;
using Catalog.API.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : BaseController
    {
        IProductManager _productManager;
        public CatalogController(IProductManager productManager)
        {
            _productManager = productManager;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public IActionResult GetProduct()
        {
            var product = _productManager.GetAll();
            return CustomResult("Product List Successfully",product,HttpStatusCode.OK);
        }
    }
}
