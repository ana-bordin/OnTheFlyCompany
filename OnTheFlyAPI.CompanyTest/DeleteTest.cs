using Microsoft.AspNetCore.Mvc;
using Mongo2Go;
using MongoDB.Driver;
using OnTheFlyAPI.Address.Controllers;
using OnTheFlyAPI.Address.Services;
using OnTheFlyAPI.Company.Controllers;
using OnTheFlyAPI.Company.Services;
using OnTheFlyAPI.Company.Utils;

namespace OnTheFlyAPI.CompanyTest
{
    public class DeleteTest
    {
        private readonly MongoDbRunner _runner;
        private readonly IMongoDatabase _database;
        private readonly CompanyService _companyService;
        private readonly CompanyController _controller;
        private readonly AddressesService _addressService;
        private readonly AddressesController _addressController;

        public DeleteTest()
        {
            _runner = MongoDbRunner.Start();
            var client = new MongoClient(_runner.ConnectionString);
            CompanyAPIDataBaseSettings settings = new()
            {
                CompanyCollectionName = "Company",
                CompanyHistoryCollectionName = "CompanyHistory",
                AddressCollectionName = "Address",
                AircraftCollectionName = "Aircraft",
                ConnectionString = "mongodb://root:Mongo%402024%23@localhost:27017",
                DatabaseName = "OnTheFly"
            };
            OnTheFlyAPI.Address.Utils.CompanyAPIDataBaseSettings settingsAddress = new()
            {
                CompanyCollectionName = "Company",
                CompanyHistoryCollectionName = "CompanyHistory",
                AddressCollectionName = "Address",
                AircraftCollectionName = "Aircraft",
                ConnectionString = "mongodb://root:Mongo%402024%23@localhost:27017",
                DatabaseName = "OnTheFly"
            };
            _companyService = new CompanyService(settings);
            _addressService = new AddressesService(settingsAddress);
            _controller = new CompanyController(_companyService);
            _addressController = new AddressesController(_addressService);
        }

        [Fact]
        public async Task Delete_NotFound()
        {
            var result = await _controller.Delete("12345678000195");

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Company not found!", notFoundObjectResult.Value);
        }

        [Fact]
        public async Task Delete_CompanyExists_ReturnsOkObjectResult()
        {
            var company = new Company.Models.Company
            {
                Cnpj = "28634885000108",
                Name = "Valid Company",
                NameOpt = "Optional Name",
                DtOpen = DateTime.Now.AddYears(-1),
                Restricted = false,
                Address = new Address.Models.Address
                {
                    ZipCode = "15997080",
                    Street = "",
                    Number = 123,
                    Complement = "Apt 1",
                    City = "",
                    State = ""
                }
            };
            await _companyService.PostCompany(company);

            var result = await _controller.Delete("28634885000108");

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("Company successfully deleted!", okObjectResult.Value);
        }

        [Fact]
        public async Task Restorage_HistoryCompany_ReturnsOkObjectResult()
        {
            var result = await _controller.Restorage("09436256000110");

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("Company successfully restored!", okObjectResult.Value);
        }

        [Fact]
        public async Task Restorage_HistoryCompany_NotFound()
        {
            var result = await _controller.Restorage("20034885000108");

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Company not found!", notFoundObjectResult.Value);
        }
    }
}