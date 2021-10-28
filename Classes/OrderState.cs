using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Classes
{
    public enum OrderState : int
    {
        New = 1,
        Pending,
        Rejected,
        Complete
    }
}
