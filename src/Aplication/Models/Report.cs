using Domain.Entities;

namespace Application.Models;

public class Report
{
    public double Amount { get; set; }
    public List<Transaction> Transactions { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set;}
}
