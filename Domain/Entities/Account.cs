using Domain.Common;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Account: BaseEntity
{
    public Account()
    {
        Transactions = new List<Transaction>();
    }

    public Guid ApplicationUserId { get; set; }
    [JsonIgnore]
    public ApplicationUser ApplicationUser { get; set; } = null!;
    public double Balance {  get; set; }
    public string Name { get; set; } = null!;
    public List<Transaction> Transactions { get; set; }
}
