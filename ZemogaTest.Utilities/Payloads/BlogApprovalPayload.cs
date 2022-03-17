using System;
using System.Collections.Generic;
using System.Text;

namespace ZemogaTest.Utilities.Payloads
{
    public class BlogApprovalPayload
    {
        public int BlogId { get; set; }
        public bool Approve { get; set; }
        public int UserId { get; set; }
    }
}
