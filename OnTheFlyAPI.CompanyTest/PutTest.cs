using Microsoft.AspNetCore.Mvc;
using OnTheFlyAPI.Address.Models;
using OnTheFlyAPI.Address.Services;
using OnTheFlyAPI.Company.Controllers;
using OnTheFlyAPI.Company.Services;
using OnTheFlyAPI.Company.Utils;

namespace OnTheFlyAPI.CompanyTest
{
    public class PutTest
    {
        private ICompanyAPIDataBaseSettings _settings;
        private CompanyService _companyService;
        private CompanyController _companyController;

        public PutTest()
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
        public async Task PatchCnpjNotFound()
        {
            Company.Models.CompanyPatchDTO companyDTO = new Company.Models.CompanyPatchDTO
            {
                NameOpt = "",
                Complement = "aqui perto",
                Number = 10

            };
            //tentando achar um cnpj que nao existe na collection
            var patchCnpj = await _companyController.Patch(companyDTO, "63308382000135");
            
            Assert.Equal("Company not found!", ((ObjectResult)patchCnpj).Value);
        }
    }
}