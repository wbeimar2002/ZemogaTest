using System;
using System.Collections.Generic;
using System.Text;

namespace ZemogaTest.Utilities.Entities
{
    public class Status: BaseEntity
    {
        public int StatusId { get; set; }
        public string Description { get; set; }
        public ICollection<Blog> Blog { get; set; }
    }
}
