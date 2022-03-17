using System;
using System.Collections.Generic;
using System.Text;

namespace ZemogaTest.Utilities.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public string   CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }

    }
}
