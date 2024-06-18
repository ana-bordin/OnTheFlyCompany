using OnTheFlyAPI.Address.Models;

namespace OnTheFlyAPI.Company.Models
{
    public class CompanyPatchDTO
    {

        public string NameOpt { get; set; }
        public bool Restricted { get; set; }
        public string Complement { get; set; }

    }
}
