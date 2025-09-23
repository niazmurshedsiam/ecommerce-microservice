using Dapper;
using Discount.Grpc.Model;
using Npgsql;


namespace Discount.Grpc.Repository
{
    public class CouponRepository : ICouponRepository
    {
        IConfiguration _configuration;
        public CouponRepository(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            var connection = new NpgsqlConnection(_configuration.GetConnectionString("DiscountDB"));
            var aff = await connection.ExecuteAsync("insert into coupon(ProductId, ProductName,Description,Amount) values(@ProductId, @ProductName,@Description,@Amount)",new { ProductId = coupon.ProductId, ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount});
            if (aff > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteDiscount(string productId)
        {
            var connection = new NpgsqlConnection(_configuration.GetConnectionString("DiscountDB"));
            var del = await connection.ExecuteAsync("delete from Coupon where ProductId = @ProductId", new { ProductId = productId });
            if (del > 0) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Coupon> GetDiscount(string productId)
        {
            var connection = new NpgsqlConnection(_configuration.GetConnectionString("DiscountDB"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("select * from coupon where ProductId = @ProductId", new { ProductId = productId });
            if (coupon == null) 
            {
                return new Coupon() { Amount = 0, ProductId = "No Discount" };
            }
            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            var connection = new NpgsqlConnection(_configuration.GetConnectionString("DiscountDB"));
            var upp = await connection.ExecuteAsync("update Coupon set ProductId = @ProductId, ProductName = @ProductName,Description = @Description,Amount = @Amount", new { ProductId = coupon.ProductId, ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });
            if (upp > 0) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
