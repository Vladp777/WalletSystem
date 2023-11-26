using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class AuthenticationResult
    {
        private readonly IdentityResult _identityResult;
        public AuthenticationResult()
        {
            _identityResult = IdentityResult.Success;
        }
        public AuthenticationResult(IdentityResult identityResult)
        {
            _identityResult = identityResult;
        }
        public string? Token {  get; set; }
        public string? UserId { get; set; }

        public IEnumerable<IdentityError> Errors => _identityResult.Errors;

        public bool Succeeded => _identityResult.Succeeded;
        
    }
}
