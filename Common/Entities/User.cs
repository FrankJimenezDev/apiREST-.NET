

using System.Text.Json.Serialization;

namespace Common.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Dni { get; set; }

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
