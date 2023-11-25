using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IIdentityService
    {
        public Task<AuthenticationResult> RegisterUser(string email, string userName, string password);
        public Task<AuthenticationResult> LoginUser(string email, string password);
    }
}
