using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Evals.IV.Infrastructure.Identity
{
    
    public class UserDbContext : IdentityDbContext<Customer>
    {
        public UserDbContext()
            : base("DefaultConnection")
        {

        }
    }
}
