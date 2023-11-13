﻿using Domain.Common;
using Domain.Enums;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Transaction: BaseEntity
{
    public Guid AccountId {  get; set; }
    [JsonIgnore]
    public Account Account { get; set; } = null!;

    public TransactionType Type { get; set; }
    public string Description { get; set; } = null!;
    public double Count {  get; set; }
    public DateTime DateTime { get; set; }
    public double Result_Balance {  get; set; }
    public TransactionTag Tag { get;  }
}