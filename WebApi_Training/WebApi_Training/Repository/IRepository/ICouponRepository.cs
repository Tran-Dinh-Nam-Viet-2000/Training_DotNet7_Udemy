using WebApi_Training.Dto;
using WebApi_Training.Models;

namespace WebApi_Training.Repository.IRepository
{
    //Repository dùng để query sql data, bên Service sẽ gọi đến hàm tương ứng thay vì query thông thường 
    public interface ICouponRepository
    {
        Task<IEnumerable<Coupon>> GetAllAsync();
        Task<Coupon> GetById(int id);
        Task<Coupon> Create(CreateCouponDto createCouponDto);
        Task<Coupon> Update(UpdateCouponDto updateCouponDto, int id);
        void DeleteById(int id);
    }
}
