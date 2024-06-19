using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnTheFlyAPI.Address.Models;
using OnTheFlyAPI.Address.Services;
using OnTheFlyAPI.Address.Utils;
using OnTheFlyAPI.Company.Controllers;
using OnTheFlyAPI.Company.Services;
using OnTheFlyAPI.Company.Utils;
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
            config.DatabaseName = "OnTheFly";
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
        public async Task CreateCompany()
        {
            Company.Models.CompanyDTO company = new Company.Models.CompanyDTO
            {
                Cnpj = "43.241.060/0001-09",
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
            await _companyController.Post(company);

            bool resultado = false;
            if (company != null)
                resultado = true;
            Debug.Print(company.Name);

            Assert.True(resultado);
        }
    }
}