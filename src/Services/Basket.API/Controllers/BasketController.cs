using Basket.API.GrpcServices;
using Basket.API.Models;
using Basket.API.Repositories;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasketController : BaseController
    {
        IBasketRepository _basketRepository;
        DiscountGrpcService _discountGrpcService;
        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService) 
        {
            _basketRepository = basketRepository;
            _discountGrpcService = discountGrpcService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ShoppingCart),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBasket(string userName) 
        {
            
            try
            {
                
                var basket = await _basketRepository.GetBasket(userName);
                return CustomResult("Basket Load Data Successfully.",basket,HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {

                return CustomResult("Basket Data Not Found", HttpStatusCode.BadRequest);
            }

        }
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBasket([FromBody]ShoppingCart basket)
        {
            try
            {
                foreach (var item in basket.Items)
                {
                    var coupon = await _discountGrpcService.GetDiscount(item.ProductId);
                    item.Price -= coupon.Amount;
                }
                return  CustomResult("Basket Update Successfully.",await _basketRepository.UpdateBasket(basket));
            }
            catch (Exception ex)
            {

                return CustomResult("Basket Updated UnSuccessfully.", HttpStatusCode.BadRequest);
            }
        }
        [HttpDelete]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            try
            {
                await _basketRepository.DeleteBasket(userName);
                return CustomResult("Basket Data Delete.");
            }
            catch (Exception ex)
            {
                return CustomResult("Basket Delete UnSuccessfully.", HttpStatusCode.BadRequest);
            }
        }
    }
}
