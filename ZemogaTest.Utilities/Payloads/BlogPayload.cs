using System;
using System.Collections.Generic;
using System.Text;

namespace ZemogaTest.Utilities.Payloads
{
    public class BlogPayload
    {
        public int? BlogId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public int? StatusId { get; set; }
    }
}
