using System;

namespace Domain.Entities;

public class TransactionType
{
    //private readonly static Lazy<TransactionType> _income = 
    //    new Lazy<TransactionType>(() => 
    //            new TransactionType 
    //            { 
    //                Id = 1, 
    //                Type = "Income" 
    //            });

    //private readonly static Lazy<TransactionType> _expence = 
    //    new Lazy<TransactionType>(() => 
    //            new TransactionType
    //            {
    //                Id = 2,
    //                Type = "Expence"
    //            });


    public int Id { get; set; }
    public string Type { get; set; } = null!;

    private TransactionType() { }

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

    public override bool Equals(object obj)
    {
        return Equals((TransactionType)obj);
    }
    public bool Equals(TransactionType other)
    {
        return Id == other.Id && Type == other.Type;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() + Type.GetHashCode();
    }
}