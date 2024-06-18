using MongoDB.Driver;
using OnTheFlyAPI.Company.Utils;

namespace OnTheFlyAPI.Company.Services
{
    public class Post
    {
        private readonly IMongoCollection<Models.Company> _companyCollection;
        private readonly IMongoCollection<Models.Company> _companyHistoryCollection;

        public Post(ICompanyAPIDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _companyCollection = database.GetCollection<Models.Company>(settings.CompanyCollectionName);
            _companyHistoryCollection = database.GetCollection<Models.Company>(settings.CompanyHistoryCollectionName);
        }

        public async Task<Models.Company> PostCompany(Models.Company company)
        {
            if(company != null)
                _companyCollection.InsertOne(company);

            return company;
        }

        public async Task<Models.Company> PostHistoryCompany(Models.Company company)
        {
            if (company != null)
                _companyHistoryCollection.InsertOne(company);

            return company;
        }
    }
}
