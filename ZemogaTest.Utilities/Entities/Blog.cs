using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZemogaTest.Utilities.Entities
{
    public class Blog: BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int StatusId { get; set; }
        public int AuthorId { get; set; }

        public Status Status { get; set; }
        public User Author { get; set; }

    }
}
