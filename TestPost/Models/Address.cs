using Newtonsoft.Json;
using System.Reflection.Metadata;

namespace TestPost.Models
{
    public class Address
    {
        [JsonProperty("cep")]
        public string ZipCode { get; set; }
        [JsonProperty("logradouro")]
        public string Street { get; set; }
        public int Number {  get; set; }
        [JsonProperty("complemento")]
        public string Complement {  get; set; }
        [JsonProperty("localidade")]
        public string City {  get; set; }
        [JsonProperty("uf")]
        public string State { get; set; }

    }
}
