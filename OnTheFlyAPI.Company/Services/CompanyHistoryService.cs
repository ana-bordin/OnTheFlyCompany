using MongoDB.Driver;
using OnTheFlyAPI.Company.Utils;

namespace OnTheFlyAPI.Company.Services
{
    public class CompanyService
    {
        private readonly IMongoCollection<Models.Company> _company;

        public CompanyService(ICompanyAPIDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _company = database.GetCollection<Models.Company>(settings.CompanyCollectionName);
        }

        public List<Models.Company> GetAll() => _company.Find(customer => true).ToList();

        public Models.Company Get(string cnpj) => _company.Find<Models.Company>(customer => customer.Cnpj == cnpj).FirstOrDefault();

        public Models.Company Create(Models.Company customer)
        {
            _company.InsertOne(customer);
            return customer;
        }

        public Models.Company Update(Models.Company customer)
        {
            _company.ReplaceOne(c => c.Cnpj == customer.Cnpj, customer);
            return customer;
        }

        public void Delete(string cnpj)
        {
            _company.DeleteOne(c => c.Cnpj == cnpj);
        }
    }
}
