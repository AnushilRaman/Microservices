﻿using Microservices.Services.EmailAPI.Models.Dto;

namespace Microservices.Services.EmailAPI.Service.IService
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
    }
}
