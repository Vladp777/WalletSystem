using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationUser: IdentityUser
{
    public ApplicationUser()
    {
        Accounts = new List<Account> { };
    }
    public string Name {  get; set; }
    public List<Account> Accounts {  get; set; }
}
