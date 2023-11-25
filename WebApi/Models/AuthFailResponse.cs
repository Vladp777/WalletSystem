using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    internal class AuthFailResponse
    {
        public List<IdentityError> Errors { get; set; }
    }
}
