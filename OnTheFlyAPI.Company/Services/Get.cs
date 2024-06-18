using Microsoft.AspNetCore.Mvc;
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
                return await _companyCollection.Find(c => true).ToListAsync();
            }
            else if (param == 1)
            {
                return await _companyHistoryCollection.Find(c => true).ToListAsync();
            }
            return null;
        }

        public async Task<Models.Company> GetByCnpj(int param, string cnpj)
        {
            cnpj = Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
            if (param == 0)
            {
                return await _companyCollection.Find(c => c.Cnpj == cnpj).FirstOrDefaultAsync();
            }
            else if (param == 1)
            {
                return await _companyHistoryCollection.Find(c => c.Cnpj == cnpj).FirstOrDefaultAsync();
            }
            return null;
        }

        public async Task<Models.Company> GetByName(int param, string name)
        {
            name = name.Replace("+", " ");
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