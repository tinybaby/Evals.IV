using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Evals.IV.Infrastructure.Identity
{
    public class Customer : IdentityUser
    {
        private Guid _id = GuidUtil.NewComb();

        public override string Id
        {
            get
            {
                return _id.ToString();
            }
            set
            {
                _id = new Guid(value);
            }
        }


        public string LastLoginIp { get; set; }

        public DateTime LastLoginDate { get; set; }

        public string UserAvatar { get; set; }
        

    }
}
