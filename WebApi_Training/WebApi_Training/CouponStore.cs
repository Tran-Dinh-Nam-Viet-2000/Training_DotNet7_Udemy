using System.Collections.Generic;
using WebApi_Training.Models;

namespace WebApi_Training
{
    public class CouponStore
    {
        public static List<Coupon> coupons = new List<Coupon>()
        {
            new Coupon { Id =1, Name = "Tivi", Percent = 10, IsActive = true},
            new Coupon { Id =2, Name = "Television", Percent = 20, IsActive = false},
        };
        
    }
}
