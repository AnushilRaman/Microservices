namespace Microservices.Services.CartAPI.Models.Dto
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailsDto>? cartDetails { get; set; }
    }
}
