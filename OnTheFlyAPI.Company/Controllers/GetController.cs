using Microsoft.AspNetCore.Mvc;
using OnTheFlyAPI.Company.Services;

namespace OnTheFlyAPI.Company.Controllers
{
    public class GetController : Controller
    {
        private readonly Get _getService;

        public GetController(Get getService)
        {
            _getService = getService;
        }

        public async Task<List<Models.Company>> GetAll(int param)
        {
            return await _getService.GetAll(param);
        }

        public async Task<Models.Company> GetByCnpj(int param, string cnpj)
        {
            return await _getService.GetByCnpj(param, cnpj);
        }

        public async Task<Models.Company> GetByName(int param, string name)
        {
            return await _getService.GetByCnpj(param, name);
        }
    }
}
