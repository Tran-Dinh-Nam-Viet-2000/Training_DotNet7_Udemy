namespace WebApi_Training.Dto
{
    public class CreateCouponDto
    {
        public string Name { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Created { get; set; }
    }
}
