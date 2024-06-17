using OnTheFlyAPI.Address.Models;

namespace OnTheFlyAPI.Company.Models
{
    public class Company
    {
        public string Cnpj { get; set; }
        public string Name { get; set; }
        public string NameOpt { get; set; }
        public DateOnly DtOpen { get; set; }
        public bool Restricted { get; set; }
        public OnTheFlyAPI.Address.Models.Address Address { get; set; }

        
    }
}
