using Catalog.API.Interface.Manager;
using Catalog.API.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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
        [ResponseCache(Duration = 30)]
        public IActionResult GetProduct()
        {
            try
            {
                var product = _productManager.GetAll();
                return CustomResult("Product List Successfully", product, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return CustomResult(ex.Message,HttpStatusCode.BadRequest);
            }
            
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ResponseCache(Duration = 30)]
        public IActionResult GetByCategory(string category)
        {
            try
            {
                var product = _productManager.GetCategory(category);
                return CustomResult("Category List Successfully", product, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }

        }

        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult GetProductId(string id)
        {
            try
            {
                var product = _productManager.GetById(id);
                return CustomResult("Single Product Successfully.", product, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }

        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult CreateProduct([FromBody] Product product) 
        {
            try
            {
                product.Id = ObjectId.GenerateNewId().ToString();
                bool isSaved = _productManager.Add(product);
                if (isSaved)
                {
                    return CustomResult("Product Created Successfully", product, HttpStatusCode.Created);
                }
                return CustomResult("Product Created UnSuccessfully", product, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {

               return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            try
            {
                if (string.IsNullOrEmpty(product.Id))
                {
                    return CustomResult("Product Not Found",product,HttpStatusCode.NotFound);
                }
                bool isUpdated = _productManager.Update(product.Id, product);
                if (isUpdated) 
                {
                    return CustomResult("Product Update Successfully",product,HttpStatusCode.OK);
                }
                return CustomResult("Product Update UnSuccessfully", product,HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {

                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult DeleteProduct(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return CustomResult("Product Not Found",  HttpStatusCode.NotFound);
                }
                bool isDeleted = _productManager.Delete(id);
                if (isDeleted)
                {
                    return CustomResult("Product Delete Successfully", HttpStatusCode.OK);
                }
                return CustomResult("Product Delete UnSuccessfully", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {

                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }
    }
}
