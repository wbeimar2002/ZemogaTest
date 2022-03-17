using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZemogaTest.Utilities.Entities
{
    public class User: BaseEntity
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RolId { get; set; }

        public Rol Rol  { get; set; }

        public ICollection<Blog> Blog { get; set; }

    }
}
