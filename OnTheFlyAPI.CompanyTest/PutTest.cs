using OnTheFlyAPI.Company.Controllers;
using OnTheFlyAPI.Company.Services;

namespace OnTheFlyAPI.CompanyTest
{
    public class PutTest
    {
        public readonly CompanyService _companyService;

        [Fact]
        public void TestPatch()
        {
            CompanyController controller = new CompanyController(_companyService);
            var company = new Company.Models.CompanyPatchDTO
            {
                NameOpt = "Comp123",
                    Complement = "Comp123",
            };
                
            var result = controller.Patch(company, "09436256000110");
            Assert.NotNull(result);

        }
    }
}