using OnTheFlyAPI.Address.Models;

namespace OnTheFlyAPI.Company.Models
{
    public class CompanyDTO
    {
        public string Cnpj { get; set; }
        public string Name { get; set; }
        public string NameOpt { get; set; }
        //public DateOnly DtOpen { get; set; }
        public DateTime DtOpen { get; set; }
        public bool Restricted { get; set; }
        public AddressDTO Address { get; set; }
    }
}
