using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class TransactionTag
{
    public int Id { get; set; }
    public string Tag { get; set; } = null!;

    public static TransactionTag Transfer =>
        new TransactionTag
        {
            Id = 1,
            Tag = "Transfer"
        };

}
