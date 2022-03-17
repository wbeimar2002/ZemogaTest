using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZemogaTest.Repositories.Repositories;
using ZemogaTest.Services.Interfaces;
using ZemogaTest.Utilities.Dtos;
using ZemogaTest.Utilities.Entities;
using ZemogaTest.Utilities.Payloads;

namespace ZemogaTest.Sevices.Blogs
{

    public class BlogService : IBlogService
    {
        private readonly IBlogEngineRepository<Blog> _blogRepository;

        public BlogService(IBlogEngineRepository<Blog> blogEngineRepository)
        {
            _blogRepository = blogEngineRepository;
        }

        public IEnumerable<BlogDto> GetBlogsByStatus(List<string> status, int? authorId)
        {
            try
            {
                string[] statusArr = status.ToArray();
                if (authorId != null)
                {
                    return from p in GetAllBlogs().Where(c=>c.AuthorId == authorId)
                           where (statusArr.Contains(p.StatusDescription))
                           select p;
                }
                else
                {
                    return from p in GetAllBlogs()
                           where (statusArr.Contains(p.StatusDescription))
                           select p;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveBlogApproval(BlogApprovalPayload blogApproval)
        {
            try
            {
                var result = false;
                var blog = new Blog();

                blog = _blogRepository.GetById(blogApproval.BlogId);

                if (blogApproval.Approve)
                {
                    blog.StatusId = 2;
                }
                else
                {
                    blog.StatusId = 3;
                }
                blog.ModifiedAt = DateTime.Now;
                blog.ModifiedBy = blogApproval.UserId.ToString();
                result = _blogRepository.Update(blog);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Save(BlogPayload blogPayload)
        {
            try
            {
                var result = false;
                if (blogPayload.BlogId != null)
                {
                    result = _blogRepository.Update(BlogMap(blogPayload, false));
                }
                else
                {
                    result = _blogRepository.Add(BlogMap(blogPayload, true));

                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private Blog BlogMap(BlogPayload blogPayload, bool isCreate)
        {
            Blog result = new Blog();
            if (isCreate)
            {

                result.StatusId = 5;
                result.CreatedAt = DateTime.Now;
                result.CreatedBy = blogPayload.AuthorId.ToString();
            }
            else
            {
                result = _blogRepository.GetById(blogPayload.BlogId.Value);
                if(blogPayload.StatusId != null)
                    result.StatusId = blogPayload.StatusId.Value;
                result.ModifiedAt = DateTime.Now;
                result.ModifiedBy = blogPayload.AuthorId.ToString();
            }    

            if(blogPayload.Title != null)
                result.Title = blogPayload.Title;
            if (blogPayload.Content != null)
                result.Content = blogPayload.Content;
            result.AuthorId = blogPayload.AuthorId;

            return result;
        }

        private IEnumerable<BlogDto> GetAllBlogs()
        {
            try
            {
                var result = _blogRepository.GetAllWithInclude("Author", "Status");
                return result.Select(x => new BlogDto
                {
                    BlogId = x.BlogId,
                    AuthorFullName = x.Author.FullName,
                    Content = x.Content,
                    StatusDescription = x.Status.Description,
                    Title = x.Title,
                    SubmitDate = x.CreatedAt,
                    AuthorId = x.AuthorId
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
