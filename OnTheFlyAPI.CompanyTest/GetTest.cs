using OnTheFlyAPI.Company.Controllers;
using OnTheFlyAPI.Company.Services;

namespace OnTheFlyAPI.CompanyTest
{
    public class GetTest
    {
        public readonly CompanyService _companyService;

        [Fact]
        public void TestGetAll()
        {
            CompanyController controller = new CompanyController(_companyService);
            var result = controller.GetAll(0);
            Assert.NotNull(result);
        }
    }
}