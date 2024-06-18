using MongoDB.Driver;
using OnTheFlyAPI.Company.Utils;

namespace OnTheFlyAPI.Company.Services
{
    public class Put
    {
        private readonly IMongoCollection<Models.Company> _companyCollection;
        public Put(ICompanyAPIDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _companyCollection = database.GetCollection<Models.Company>(settings.CompanyCollectionName);

        }
        public async Task<Models.Company> Update(Models.Company company) 
        { 
            _companyCollection.ReplaceOne(c => c.Cnpj == company.Cnpj, company);

            return company;
        }

    }
}
