using Microsoft.AspNetCore.Mvc;
using OnTheFlyAPI.Company.Services;

namespace OnTheFlyAPI.Company.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [Route("api/delete")]
    [ApiController]
    public class DeleteController : Controller
    {
        private readonly Delete _companyService;
        //private readonly AddressService _addressService;
        public DeleteController(Delete companyService)
        {
            _companyService = companyService;
        }

        [HttpDelete("delete/{cnpj}")]      
        public ActionResult<Models.Company> Delete(string cnpj)
        {
            cnpj = Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
            var companyResult = _companyService.GetByCnpj(0, cnpj);
            if (cnpj == null)
                return NotFound("Companhia não encontrada!");
            
            Models.Company company = companyResult.Result;

            var inserted = _companyService.PostHistoryCompany(company);
            
            if (inserted == null)
                return BadRequest("Houve um problema para mover a companhia");

            var deleted = _companyService.DeleteCompany(cnpj);
            
            if (deleted == false)
                return BadRequest("Houve um problema para deletar a companhia");
            
            return Ok(deleted);
            
        }

        [HttpDelete("restorage/{cnpj}")]
        public ActionResult<Models.Company> Restorage(string cnpj)
        {
            cnpj = Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
            var company = _companyService.GetByCnpj(1, cnpj);
            if (cnpj == null)
                return NotFound("Companhia não encontrada!");

            var inserted = _companyService.PostCompany(company.Result);

            if (inserted == null)
                return BadRequest("Houve um problema para mover a companhia");

            var deleted = _companyService.DeleteCompany(cnpj);

            if (deleted == null)
                return BadRequest("Houve um problema para restaurar a companhia");

            return Ok(deleted);

        }
    }
}
