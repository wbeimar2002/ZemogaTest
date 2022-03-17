using System;
using System.Collections.Generic;
using System.Text;

namespace ZemogaTest.Utilities.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public int RoleId { get; set; }
    }
}
