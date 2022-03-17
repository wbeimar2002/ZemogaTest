using System;
using System.Collections.Generic;
using System.Text;
using ZemogaTest.Utilities.Dtos;

namespace ZemogaTest.Utilities.Helpers
{
    public static class ExtensionMethods
    {
        public static UserDto WithoutPassword(this UserDto user)
        {
            user.Password = null;
            return user;
        }
    }
}
