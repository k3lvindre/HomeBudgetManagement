using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudgetManagement.SharedKernel
{
    public interface IAggregateRoot
    {
        int Id { get; }
    }
}
