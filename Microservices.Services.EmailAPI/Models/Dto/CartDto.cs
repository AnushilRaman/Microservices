﻿namespace Microservices.Services.EmailAPI.Models.Dto
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailsDto>? cartDetails { get; set; }
    }
}
