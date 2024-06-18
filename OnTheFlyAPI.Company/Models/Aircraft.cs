using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace OnTheFlyAPI.Company.Models
{
    public class Aircraft
    {
        //[BsonId] // test
        public string Rab { get; set; }
        public int Capacity { get; set; }
        public DateTime DTRegistry { get; set; }
        public DateTime DTLastFlight { get; set; }
        public string CnpjCompany { get; set; }
    }
}
