using Microsoft.AspNetCore.Mvc;
using OnTheFlyAPI.Company.Services;

namespace OnTheFlyAPI.Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly Post _postService;

        public PostController(Post postService)
        {
            _postService = postService;
        }

        [HttpPost]
        public async Task<Models.Company> Post(Models.Company company)
        {
            return await _postService.PostCompany(company);
        }
    }
}
