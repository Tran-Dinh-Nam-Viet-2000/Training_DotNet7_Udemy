using Microsoft.EntityFrameworkCore;
using WebApi_Training.Database;
using WebApi_Training.Dto;
using WebApi_Training.Models;
using WebApi_Training.Repository.IRepository;

namespace WebApi_Training.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _db;

        public CouponRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Coupon> Create(CreateCouponDto createCouponDto)
        {
            if (createCouponDto == null)
            {
                return null;
            }
            var create = new Coupon
            {
                Created = createCouponDto.Created,
                Name = createCouponDto.Name,
                Percent = createCouponDto.Percent,
            };
            _db.Add(create);
            await _db.SaveChangesAsync();
            return create;
        }

        public void DeleteById(int id)
        {
            var record = _db.coupons.FirstOrDefault(x => x.Id == id);
            _db.coupons.Remove(record);
            _db.SaveChanges();
        }

        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            return await _db.coupons.ToListAsync();
        }

        public async Task<Coupon> GetById(int id)
        {
            if (id.Equals(""))
            {
                return null;
            }
            return await _db.coupons.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<Coupon> Update(UpdateCouponDto updateCouponDto, int id)
        {
            var record = _db.coupons.FirstOrDefault(n => n.Id == id);
            if (record == null)
            {
                return null;
            }
            record.Name = updateCouponDto.Name;
            record.LastUpdated = updateCouponDto.LastUpdated;
            record.Percent = updateCouponDto.Percent;
            record.IsActive = updateCouponDto.IsActive;
            await _db.SaveChangesAsync();

            return record;
        }
    }
}
