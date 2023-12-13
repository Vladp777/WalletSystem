using System;

namespace Domain.Entities;

public class TransactionType
{
    public int Id { get; set; }
    public string Type { get; set; } = null!;

    public static TransactionType Expence => 
        new ()
            {
                Id = 2,
                Type = "Expence"
            };

    public static TransactionType Income => 
        new ()
            {
                Id = 1,
                Type = "Income"
            };

}