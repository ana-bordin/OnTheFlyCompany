using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnTheFlyAPI.Company.Services;
using OnTheFlyAPI.Company.Models;
using OnTheFlyAPI.Address.Models;
using Microsoft.AspNetCore.Mvc;
using OnTheFlyAPI.Company.Utils;
using MongoDB.Driver;
using System.Runtime;
using OnTheFlyAPI.Company.Controllers;

namespace OnTheFlyAPI.CompanyTest
{
    public class GetTest
    {
        private ICompanyAPIDataBaseSettings _settings;
        private CompanyService _companyService;
        private CompanyController _companyController;

        public GetTest()
        {
            _settings = new CompanyAPIDataBaseSettings();
            _settings.DatabaseName = "OnTheFly";
            _settings.ConnectionString = "mongodb://root:Mongo%402024%23@localhost:27017";
            _settings.CompanyCollectionName = "Company";
            _settings.CompanyHistoryCollectionName = "CompanyHistory";
            _settings.AddressCollectionName = "Address";
            _settings.AircraftCollectionName = "Aircraft";
            _companyService = new CompanyService(_settings);
            _companyController = new(_companyService);
        }

        [Fact]
        public async Task TestGetByCnpj()
        {
            var getByCnpj = await _companyController.GetByCnpj(0, "09436256000110");
            Assert.IsType<OkObjectResult>(getByCnpj.Result);
        }

        [Fact]
        public async Task TestGetByCnpjBadRequest()
        {
            //bad request nos get's seria quando o usuario passa um parametro diferente de 0 ou 1
            var getByCnpj = await _companyController.GetByCnpj(2, "09436256000110");
            Assert.IsType<BadRequestObjectResult>(getByCnpj.Result);
        }

        [Fact]
        public async Task TestGetByCnpjNotFound()
        {
            //tentando achar um cnpj que nao existe na collection
            var getByCnpj = await _companyController.GetByCnpj(1, "09436256000110");
            Assert.IsType<NotFoundObjectResult>(getByCnpj.Result);
        }

        [Fact]
        public async Task TestGetAll()
        {
            var getAll = await _companyController.GetAll(0);
            Assert.IsType<OkObjectResult>(getAll.Result);
        }

        [Fact]
        public async Task TestGetAllNotFound()
        {
            //este teste so retorna notFound quando a collection esta vazia
            var getAll = await _companyController.GetAll(1);
            Assert.IsType<NotFoundObjectResult>(getAll.Result);
        }

        [Fact]
        public async Task TestGetAllBadRequest()
        {
            var getAll = await _companyController.GetAll(2);
            Assert.IsType<BadRequestObjectResult>(getAll.Result);
        }
    }
}