using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OnTheFlyAPI.Address.Models;
using OnTheFlyAPI.Address.Services;
namespace OnTheFlyAPI.Address.Controllers
{
    [Route("api/endereco")]
    [ApiController]
    public class AddressesController : ControllerBase
    {

        private readonly AddressesService _service;

        public AddressesController(AddressesService addressesService)
        {
            _service = addressesService;
        }


        [HttpPost]
        public async Task<ActionResult<Models.Address>> Post(AddressDTO dto)
        {
            //var dto = JsonConvert.DeserializeObject<Address.Models.AddressDTO>(Address);

            Models.Address? address = new Models.Address();
            address = await _service.RetrieveAdressAPI(dto);
            if (address == null)
            {
                return NotFound();
            } else
            {
                return Ok(address);
            }
        }
    }
}
