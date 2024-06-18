using Microsoft.AspNetCore.Mvc;
using OnTheFlyAPI.Company.Services;

namespace OnTheFlyAPI.Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteController : ControllerBase
    {
        private readonly CompanyService _companyService;
        private readonly CompanyHistoryService _companyHistoryService;
        //private readonly AddressService _addressService;
        public DeleteController(CompanyService companyService, CompanyHistoryService companyHistoryService)
        {
            _companyService = companyService;
            _companyHistoryService = companyHistoryService;
        }

        [HttpDelete]      
        public ActionResult<Models.Company> Delete(string cnpj)
        {
            var company = _companyService.GetByCnpj(0, cnpj);
            if (cnpj == null)
                return NotFound();
            
            var inserted = _companyService.PostHistoryCompany(company.Result);
            
            if (inserted == null)
                return BadRequest();

            var deleted = _companyService.Delete(cnpj);
            
            if (deleted == false)
                return BadRequest();
            
            return Ok(deleted);
            
        }

        [HttpDelete]
        public ActionResult<Models.Company> Restorage(string cnpj)
        {
            var company = _companyService.GetByCnpj(1, cnpj);
            if (cnpj == null)
                return NotFound();

            var inserted = _companyService.PostCompany(company.Result);

            if (inserted == null)
                return BadRequest();

            var deleted = _companyService.Delete(cnpj);

            if (deleted == null)
                return BadRequest();

            return Ok(deleted);

        }
    }
}
