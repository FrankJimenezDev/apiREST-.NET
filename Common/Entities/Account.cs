
using System.Text.Json.Serialization;

namespace Common.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public decimal Balance { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
