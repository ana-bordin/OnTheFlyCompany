using Microsoft.AspNetCore.Mvc;
using TestPost.Models;
using TestPost.Services;
namespace TestPost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly Services.Post _postService;
        private readonly AddressService _addressService;

        public PostController(Services.Post postService, AddressService addressService)
        {
            _postService = postService;
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<ActionResult<Company>> Get(Company company)
        {
            return company;
        }

        [HttpPost]
        public async Task<ActionResult<Company>> Post(CompanyDTO dto)
        {
            Company company;
            try
            {
                dto.Address.ZipCode = string.Join("", dto.Address.ZipCode.Where(Char.IsDigit)); // Formata pra vir apenas números do CEP
                if(dto.Address.ZipCode.Length != 8)
                    return Problem($"CEP ({dto.Address.ZipCode}) diferente de 8 dígitos!");

                if (!Company.VerificarCnpj(dto.Cnpj))
                    return Problem("CNPJ Inválido!");

                var address = await _addressService.RetrieveAdressAPI(dto.Address);
                if (address == null)
                    return Problem("CEP Inválido!");

                company = new(dto);
                company.Address = address;
                var result = await _postService.PostCompany(company);
            }
            catch (Exception ex)
            {
                throw;
            }

            return CreatedAtAction("Get", company, company);
        }
    }
}
