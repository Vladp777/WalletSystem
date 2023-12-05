using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Account: BaseEntity
{
    public Account()
    {
        Transactions = new List<Transaction>();
    }

    public string UserId { get; set; }
    [JsonIgnore]
    public ApplicationUser User { get; set; } = null!;
    public double Balance {  get; set; }
    public string Name { get; set; } = null!;
    public List<Transaction> Transactions { get; set; }
}
