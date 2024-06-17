using Microsoft.AspNetCore.Mvc;
using TestPost.Models;
using TestPost.Services;
namespace TestPost.Controllers
{
    [Route("api/Company")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly Post _postService;
        private readonly AddressService _addressService;

        public PostController(Post postService, AddressService addressService)
        {
            _postService = postService;
            _addressService = addressService;
        }

        [HttpPost]
        public async Task<ActionResult<Company>> Post(CompanyDTO dto)
        {
            Company company;
            try
            {
                if (!Company.VerificarCnpj(dto.Cnpj))
                    return BadRequest("CNPJ Inválido!");

                var address = await _addressService.RetrieveAdressAPI(dto.Address);
                if (address == null)
                    return BadRequest("CEP Inválido!");

                company = new(dto);
                company.Address = address;
                var result = await _postService.PostCompany(company);
            }
            catch (Exception ex)
            {
                throw;
            }

            return company;
        }
    }
}
