using MongoDB.Driver;
using TestPost.Utils;

namespace TestPost.Services
{
    public class Post
    {
        private readonly IMongoCollection<Company> _companyCollection;
        private readonly IMongoCollection<Company> _companyHistoryCollection;

        public Post(ICompanyAPIDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _companyCollection = database.GetCollection<Company>(settings.CompanyCollectionName);
            _companyHistoryCollection = database.GetCollection<Company>(settings.CompanyHistoryCollectionName);
        }

        public async Task<Company> PostCompany(Company company)
        {
            if(company != null)
                _companyCollection.InsertOne(company);

            return company;
        }
    }
}
