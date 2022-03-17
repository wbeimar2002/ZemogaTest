using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ZemogaTest.Utilities.Dtos;
using ZemogaTest.Utilities.Payloads;
using ZemogaTest.WebApp.Models;

namespace ZemogaTest.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<UserDto>  ValidateLogin(string username, string password)
        {
            UserDto userDto = null;
            using (var client = new HttpClient())
            {
                UserPayload userPayload = new UserPayload { UserName = username, PassWord = password };
                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                var jsondata = JsonConvert.SerializeObject(userPayload, serializerSettings);

                HttpContent httpContent = new StringContent(jsondata, Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync("https://localhost:44307/api/Users/authenticate", httpContent);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    userDto = JsonConvert.DeserializeObject<UserDto>(result);
                }
            }

            return userDto;
        }

        [HttpPost]
        public async Task<bool> Save(int? blogId, string blogTitle, string blogContent, int authorId, int? statusId)
        {
            bool res = false;
            using (var client = new HttpClient())
            {
                BlogPayload blogPayload = new BlogPayload {Title = blogTitle, Content = blogContent, AuthorId = authorId };

                if (!string.IsNullOrEmpty(blogId.ToString()))
                {
                    blogPayload.BlogId = blogId;
                }
                if (!string.IsNullOrEmpty(statusId.ToString()))
                {
                    blogPayload.StatusId = statusId;
                }
                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                var jsondata = JsonConvert.SerializeObject(blogPayload, serializerSettings);

                HttpContent httpContent = new StringContent(jsondata, Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync("https://localhost:44307/api/Blog/CreateUpdateBlog", httpContent);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    res = bool.Parse(result);
                }
            }

            return res;
        }

        [HttpGet]
        public async Task<IEnumerable<BlogDto>> AnominousAccess()
        {
            List<BlogDto> blogDtos = new List<BlogDto>();
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44307/api/Blog/GetApprovedBlogs");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    blogDtos = JsonConvert.DeserializeObject<List<BlogDto>>(result);
                }
            }
            return blogDtos;
        }

        [HttpGet]
        public async Task<IEnumerable<BlogDto>> GetBlogsByAutor(int authorId)
        {
            List<BlogDto> blogDtos = new List<BlogDto>();
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44307/api/Blog/GetBlogsByAuthor?authorId=" + authorId.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    blogDtos = JsonConvert.DeserializeObject<List<BlogDto>>(result);
                }
            }
            return blogDtos;
        }

        [HttpGet]
        public async Task<IEnumerable<BlogDto>> GetBlogsPendingToApproval()
        {
            List<BlogDto> blogDtos = new List<BlogDto>();
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:44307/api/Blog/GetBlogsPendingToApproval/");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    blogDtos = JsonConvert.DeserializeObject<List<BlogDto>>(result);
                }
            }
            return blogDtos;
        }

        [HttpDelete]
        public async Task<bool> DeleteBlog(int blogId, int authorId)
        {
            return await Save(blogId,null,null,authorId,4);
        }


        [HttpPost]
        public async Task<bool> SaveBlogApproval(int blogId, int authorId, bool approve)
        {
            bool res = false;
            using (var client = new HttpClient())
            {
                BlogApprovalPayload blogApprovalPayload = new BlogApprovalPayload { BlogId= blogId, UserId=authorId, Approve=approve };

                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                var jsondata = JsonConvert.SerializeObject(blogApprovalPayload, serializerSettings);

                HttpContent httpContent = new StringContent(jsondata, Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync("https://localhost:44307/api/Blog/SaveBlogApproval", httpContent);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    res = bool.Parse(result);
                }
            }

            return res;
        }
        
    }
}
