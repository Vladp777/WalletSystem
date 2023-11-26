using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    internal class AuthFailResponse
    {
        public IEnumerable<IdentityError> Errors { get; set; }
    }
}
