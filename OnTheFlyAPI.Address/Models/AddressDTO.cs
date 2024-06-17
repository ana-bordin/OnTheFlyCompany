using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OnTheFlyAPI.Address.Models
{
    public class AddressDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Description { get; set; }
        public string CEP { get; set; }
    }
}
