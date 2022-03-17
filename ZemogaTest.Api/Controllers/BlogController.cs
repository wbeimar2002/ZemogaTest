using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ZemogaTest.Services.Interfaces;
using ZemogaTest.Utilities.Dtos;
using ZemogaTest.Utilities.Entities;
using ZemogaTest.Utilities.Payloads;

namespace ZemogaTest.Api.Controllers
{
    //#if !DEBUG
    //[Authorize]
    //#endif
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        [Route("GetBlogsPendingToApproval")]
        public IEnumerable<BlogDto> GetBlogsPendingToApproval()
        {
            var result = _blogService.GetBlogsByStatus(new List<string> { "PendingToApproval" }, null);
            return result;
        }

        [HttpGet]
        [Route("GetBlogsByAuthor")]
        public IEnumerable<BlogDto> GetBlogsByAuthor(int authorId)
        {
            var result = _blogService.GetBlogsByStatus(new List<string> { "Created", "Approved", "Rejected" }, authorId);
            return result;
        }

        [HttpGet]
        [Route("GetApprovedBlogs")]
        public IEnumerable<BlogDto> GetApprovedBlogs()
        {
            var result = _blogService.GetBlogsByStatus(new List<string> { "Approved" }, null);
            return result;
        }

        [HttpPost]
        [Route("SaveBlogApproval")]
        public bool SaveBlogApproval([FromBody] BlogApprovalPayload blogApproval)
        {
            var result = _blogService.SaveBlogApproval(blogApproval);
            return result;
        }

        [HttpPost]
        [Route("CreateUpdateBlog")]
        public bool CreateUpdateBlog([FromBody] BlogPayload blogPayload)
        {
            var result = _blogService.Save(blogPayload);
            return result;
        }

    }
}
