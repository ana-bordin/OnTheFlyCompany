using OnTheFlyAPI.Address.Models;
using OnTheFlyAPI.Company.Controllers;
using OnTheFlyAPI.Company.Models;
using OnTheFlyAPI.Company.Services;

namespace OnTheFlyAPI.CompanyTest
{
    public class PostTest
    {
        public readonly CompanyService _companyService;

        [Fact]
        public void TestPost()
        {
            CompanyController controller = new CompanyController(_companyService);
            var company = new Company.Models.CompanyDTO
            {
                Cnpj = "12345678901234",
                Name = "Company",
                NameOpt = "Company",
                DtOpen = DateTime.Now,
                Restricted = false,
                Address = new Address.Models.AddressDTO
                {
                    Number = 123,
                    Complement = "Complement",
                    ZipCode = "15997020"
                }
            };
            var result = controller.Post(company);
            Assert.NotNull(result); 
        }
    }
}