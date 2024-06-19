using OnTheFlyAPI.Address.Services;
using OnTheFlyAPI.Company.Controllers;
using OnTheFlyAPI.Company.Services;

namespace OnTheFlyAPI.CompanyTest
{
    public class PutTest
    {
        AddressesService _addressesService;
        CompanyService _companyService;
        CompanyController _companyController;

        public PutTest()
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
        public void Test1()
        {

        }
    }
}