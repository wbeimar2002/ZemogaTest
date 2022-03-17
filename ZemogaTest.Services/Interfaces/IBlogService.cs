using System;
using System.Collections.Generic;
using System.Text;
using ZemogaTest.Utilities.Dtos;
using ZemogaTest.Utilities.Entities;
using ZemogaTest.Utilities.Payloads;

namespace ZemogaTest.Services.Interfaces
{
    public interface IBlogService
    {
        IEnumerable<BlogDto> GetBlogsByStatus(List<string> status, int? authorId);
        bool SaveBlogApproval(BlogApprovalPayload blogApproval);

        bool Save(BlogPayload blogPayload);
    }
}
