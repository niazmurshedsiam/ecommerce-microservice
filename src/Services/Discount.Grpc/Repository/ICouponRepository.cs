using Discount.Grpc.Model;

namespace Discount.Grpc.Repository
{
    public interface ICouponRepository
    {
        Task<Coupon> GetDiscount(string productId);
        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productId);
    }
}
