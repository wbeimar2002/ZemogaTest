using System;
using System.Collections.Generic;
using System.Text;

namespace ZemogaTest.Utilities.Entities
{
    public class Rol: BaseEntity
    {
        public int RolId { get; set; }
        public string Name { get; set; }
        public ICollection<User> User { get; set; }
    }
}
