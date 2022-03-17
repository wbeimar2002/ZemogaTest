using System;
using System.Collections.Generic;
using System.Text;
using ZemogaTest.Utilities.Dtos;
using ZemogaTest.Utilities.Payloads;

namespace ZemogaTest.Services.Interfaces
{
    public interface IUserService
    {
        UserDto Authenticate(UserPayload userPayload);
    }
}
