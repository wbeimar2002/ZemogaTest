using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ZemogaTest.Repositories.Repositories;
using ZemogaTest.Sevices.Blogs;
using ZemogaTest.Utilities.Dtos;
using ZemogaTest.Utilities.Entities;

namespace ZemogaTest.UniTests
{
    public class BlogServiceTest
    {
        private Mock<IBlogEngineRepository<Blog>> _blogEngineRepository = new Mock<IBlogEngineRepository<Blog>>();

        [Fact]
        public void GetAllBlogsTest()
        {
            var blogs = new List<Blog>();
            blogs.Add(new Blog() { BlogId = 1, Content = "test", Title = "Titletest", CreatedAt = DateTime.Now, CreatedBy = "admin", Status = new Status() { StatusId = 1, Description = "Approved" } });
            blogs.Add(new Blog() { BlogId = 1, Content = "test", Title = "Titletest", CreatedAt = DateTime.Now, CreatedBy = "admin", Status = new Status() { StatusId = 1, Description = "Approved" } });
            blogs.Add(new Blog() { BlogId = 1, Content = "test", Title = "Titletest", CreatedAt = DateTime.Now, CreatedBy = "admin", Status = new Status() { StatusId = 2, Description = "Rejected" } });

            _blogEngineRepository
                .Setup(repo => repo.GetAll()).Returns(blogs);

            var blogservice = new BlogService(_blogEngineRepository.Object);
            List<BlogDto> result = (List<BlogDto>)blogservice.GetBlogsByStatus(new List<string> { "Approved" }, null);

            Assert.AreEqual(2, result.Count);
        }
    }
}
