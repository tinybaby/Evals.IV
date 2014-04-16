using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evals.IV.Domain.DomainModules
{
    public interface IEntity
    {
        Guid Id
        {
            get;
            set;
        }
    }
}
