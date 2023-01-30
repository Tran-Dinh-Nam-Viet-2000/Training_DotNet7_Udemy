namespace WebApi_Training.Dto
{
    public class UpdateCouponDto
    {
        public string Name { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
