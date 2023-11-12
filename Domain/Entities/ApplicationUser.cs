using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationUser: IdentityUser
{
    public ApplicationUser()
    {
        Accounts = new List<Account> { };
    }
    public List<Account> Accounts {  get; set; }
}
