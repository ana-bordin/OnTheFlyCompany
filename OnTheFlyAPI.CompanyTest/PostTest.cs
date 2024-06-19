using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnTheFlyAPI.Address.Models;
using OnTheFlyAPI.Address.Services;
using OnTheFlyAPI.Company.Controllers;
using OnTheFlyAPI.Company.Services;
using System.Diagnostics;
namespace OnTheFlyAPI.CompanyTest
{
    public class PostTest
    {
        AddressesService _addressesService;
        CompanyService _companyService;
        CompanyController _companyController;

        public PostTest()
        {
            Address.Utils.ICompanyAPIDataBaseSettings config = new Address.Utils.CompanyAPIDataBaseSettings();
            config.DatabaseName = "OnTheFlyTest";
            config.CompanyCollectionName = "Company";
            config.CompanyHistoryCollectionName = "CompanyHistory";
            config.AddressCollectionName = "Address";
            config.AircraftCollectionName = "Aircraft";
            config.ConnectionString = "mongodb://root:Mongo%402024%23@localhost:27017";
            _addressesService = new(config);

            Company.Utils.ICompanyAPIDataBaseSettings config2 = new Company.Utils.CompanyAPIDataBaseSettings();
            config2.DatabaseName = config.DatabaseName;
            config2.CompanyCollectionName = config.CompanyCollectionName;
            config2.CompanyHistoryCollectionName = config.CompanyHistoryCollectionName;
            config2.AddressCollectionName = config.AddressCollectionName;
            config2.AircraftCollectionName = config.AircraftCollectionName;
            config2.ConnectionString = config.ConnectionString;
            _companyService = new(config2);

            _companyController = new(_companyService);
        }

        [Fact]
        public async Task<Address.Models.Address> CreateAddress()
        {
            AddressDTO addressDTO = new AddressDTO
            {
                ZipCode = "14802020",
                Complement = "aqui perto",
                Number = 10
            };
            var address = await _addressesService.RetrieveAdressAPI(addressDTO);

            bool resultado = false;
            if (address != null)
                resultado = true;

            Debug.Print(address.Street);

            Assert.Equal(addressDTO.ZipCode, address.ZipCode);

            return address;
        }
        [Fact]
        public async Task PostOK()
        {
            Company.Models.CompanyDTO companyDTO = new Company.Models.CompanyDTO
            {
                Cnpj = "73.660.631/0001-00",
                DtOpen = DateTime.Now,
                Name = "Empresa x",
                NameOpt = "",
                Restricted = false,
                Address = new AddressDTO
                {
                    ZipCode = "14802020",
                    Complement = "aqui perto",
                    Number = 10
                }
            };
            var result = await _companyController.Post(companyDTO);
            var okObject = result.Result as OkObjectResult;
            var createdCompany = Assert.IsAssignableFrom<Company.Models.Company>(okObject.Value);
            Assert.Equal(companyDTO.Name, createdCompany.Name);
            Assert.Equal(companyDTO.Address.ZipCode, createdCompany.Address.ZipCode);

            // Delete the company and send it to History
            await _companyController.Delete(Company.Models.Company.RemoveMask(companyDTO.Cnpj));
        }

        [Fact]
        public async Task PostAlreadyRegistered()
        {
            Company.Models.CompanyDTO companyDTO = new Company.Models.CompanyDTO
            {
                Cnpj = "73.660.631/0001-00",
                DtOpen = DateTime.Now,
                Name = "Empresa x",
                NameOpt = "",
                Restricted = false,
                Address = new AddressDTO
                {
                    ZipCode = "14802020",
                    Complement = "aqui perto",
                    Number = 10
                }
            };
            var result = await _companyController.Post(companyDTO);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.Equal("Company is already registered and it is deleted. Restore it if needed.", badRequestResult.Value);

            // Delete it from history (and dont recover it)
            _companyService.RestorageCompany(Company.Models.Company.RemoveMask(companyDTO.Cnpj));
        }

        [Fact]
        public async Task PostInvalidCNPJ()
        {
            Company.Models.CompanyDTO companyDTO = new Company.Models.CompanyDTO
            {
                Cnpj = "11.111.111/1111-11",
                DtOpen = DateTime.Now,
                Name = "Empresa Y",
                NameOpt = "",
                Restricted = false,
                Address = new AddressDTO
                {
                    ZipCode = "14802020",
                    Complement = "aqui perto",
                    Number = 10
                }
            };
            var result = await _companyController.Post(companyDTO);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.Equal("Invalid CNPJ!", badRequestResult.Value);
        }

        [Fact]
        public async Task PostInvalidName()
        {
            Company.Models.CompanyDTO companyDTO = new Company.Models.CompanyDTO
            {
                Cnpj = "55.879.508/0001-01",
                DtOpen = DateTime.Now,
                Name = "Empresa YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY",
                NameOpt = "",
                Restricted = false,
                Address = new AddressDTO
                {
                    ZipCode = "14802020",
                    Complement = "aqui perto",
                    Number = 10
                }
            };
            var result = await _companyController.Post(companyDTO);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.Equal("Name too long!", badRequestResult.Value);
        }

        [Fact]
        public async Task PostInvalidDtOpen()
        {
            Company.Models.CompanyDTO companyDTO = new Company.Models.CompanyDTO
            {
                Cnpj = "71.764.173/0001-24",
                DtOpen = new DateTime(2025,10,5),
                Name = "Empresa Y",
                NameOpt = "",
                Restricted = false,
                Address = new AddressDTO
                {
                    ZipCode = "14802020",
                    Complement = "aqui perto",
                    Number = 10
                }
            };
            var result = await _companyController.Post(companyDTO);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.Equal("Date of opening cannot be newer than current date!", badRequestResult.Value);
        }
    }
}