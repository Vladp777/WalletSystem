using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Transaction: BaseEntity
{
    public Guid AccountId {  get; set; }
    [JsonIgnore]
    public Account Account { get; set; } = null!;

    public int TypeId {  get; set; }
    public TransactionType Type { get; set; }
    public string Description { get; set; }
    public double Count {  get; set; }
    public DateTime DateTime { get; set; }
    //public double Result_Balance {  get; set; }
    public int TagId {  get; set; }
    public TransactionTag Tag { get; set; }
}
