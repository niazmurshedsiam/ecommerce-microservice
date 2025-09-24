using AutoMapper;
using Discount.Grpc.Model;
using Discount.Grpc.Protos;
using Discount.Grpc.Repository;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService: DiscountProtoService.DiscountProtoServiceBase
    {
        ICouponRepository _couponRepository;
        ILogger<DiscountService> _logger;
        IMapper _mapper;
        public DiscountService(ICouponRepository couponRepository, ILogger<DiscountService> logger, IMapper mapper) 
        {
            _couponRepository = couponRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public override async Task<CouponRequest> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _couponRepository.GetDiscount(request.ProductId);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Discount not found."));

            }
            _logger.LogInformation("Discount is retrived for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);
            // return new CouponRequest { ProductId = coupon.ProductId, ProductName = coupon.ProductName, Description = coupon.Description, Amount =  coupon.Amount};
            return _mapper.Map<CouponRequest>(coupon);
        }
        public override async Task<CouponRequest> CreateDiscount(CouponRequest request, ServerCallContext context)
        {
            var coupon =  _mapper.Map<Coupon>(request);
            bool isSaved = await  _couponRepository.CreateDiscount(coupon);
            if (isSaved)
            {
                _logger.LogInformation("Discount is successfully created. ProductName : {productName}, Amount : {amount}", coupon.ProductName);
            }
            else
            {
                _logger.LogInformation("Discount is unsuccessfully created");
            }
            return _mapper.Map<CouponRequest>(coupon);
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            bool IsDeleted = await _couponRepository.DeleteDiscount(request.ProductId);
            if (IsDeleted)
            {
                _logger.LogInformation("Discount is successfully Delete. ProductName : {productName}");
            }
            else
            {
                _logger.LogInformation("Discount is unsuccessfully Delete.");
            }
            return new DeleteDiscountResponse() { Success = IsDeleted };
        }
        public override async Task<CouponRequest> UpdateDiscount(CouponRequest request, ServerCallContext context)
        {
            var coupon =  _mapper.Map<Coupon>(request);
            bool isModify = await _couponRepository.UpdateDiscount(coupon);
            if (isModify)
            {
                _logger.LogInformation("Discount is successfully Update. ProductName : {productName}, Amount : {amount}", coupon.ProductName);
            }
            else
            {
                _logger.LogInformation("Discount is unsuccessfully Update.");
            }
            return _mapper.Map<CouponRequest>(coupon);

        }
    }
}
