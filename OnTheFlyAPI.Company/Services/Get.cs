using MongoDB.Driver;
using OnTheFlyAPI.Company.Utils;
using System.Text.RegularExpressions;

namespace OnTheFlyAPI.Company.Services
{
    public class Get
    {
        private readonly IMongoCollection<Models.Company> _companyCollection;
        private readonly IMongoCollection<Models.Company> _companyHistoryCollection;

        public Get(ICompanyAPIDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _companyCollection = database.GetCollection<Models.Company>(settings.CompanyCollectionName);
            _companyHistoryCollection = database.GetCollection<Models.Company>(settings.CompanyHistoryCollectionName);
        }

        public async Task<List<Models.Company>> GetAll(int param)
        {
            if (param == 0)
            {
                return _companyCollection.Find(c => true).ToList();
            }
            else if (param == 1)
            {
                return _companyHistoryCollection.Find(c => true).ToList();
            }
            return null;
        }

        public async Task<Models.Company> GetByCnpj(int param, string cnpj)
        {
            string numberOnly = Regex.Replace(cnpj, @"[^\d]", String.Empty);
            if (numberOnly.Length == 14)
            {
                numberOnly = Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
            }
            else
            {
                return null;
            }
            if (param == 0)
            {
                return await _companyCollection.Find(c => c.Cnpj == numberOnly).FirstOrDefaultAsync();
            }
            else if (param == 1)
            {
                return await _companyHistoryCollection.Find(c => c.Cnpj == numberOnly).FirstOrDefaultAsync();
            }
            return null;
        }

        public async Task<Models.Company> GetByName(int param, string name)
        {
            if (param == 0)
            {
                return await _companyCollection.Find(c => c.Name == name).FirstOrDefaultAsync();
            }
            else if (param == 1)
            {
                return await _companyHistoryCollection.Find(c => c.Name == name).FirstOrDefaultAsync();
            }
            return null;
        }
    }
}