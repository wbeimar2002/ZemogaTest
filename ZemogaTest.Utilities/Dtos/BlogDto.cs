using System;
using System.Collections.Generic;
using System.Text;

namespace ZemogaTest.Utilities.Dtos
{
    public class BlogDto
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string StatusDescription { get; set; }
        public int AuthorId { get; set; }
        public string AuthorFullName { get; set; }
        public DateTime SubmitDate { get; set; }
    }
}
