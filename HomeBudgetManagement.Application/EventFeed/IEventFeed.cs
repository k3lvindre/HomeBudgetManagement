using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Application.EventFeed
{
    public interface IEventFeed
    {
        void Publish();
        void Connect();
    }
}
