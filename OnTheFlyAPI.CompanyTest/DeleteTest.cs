using Mongo2Go;
using MongoDB.Driver;
using OnTheFlyAPI.Company.Controllers;
using OnTheFlyAPI.Company.Services;
using OnTheFlyAPI.Company.Utils;

namespace OnTheFlyAPI.CompanyTest
{
    public class DeleteTest
    {
        public readonly CompanyService _companyService;

        [Fact]
        public void TestDelete()
        {
            CompanyController controller = new CompanyController(_companyService);
            var result = controller.Delete("09436256000110");
            Assert.NotNull(result);
            
        }
    }
}