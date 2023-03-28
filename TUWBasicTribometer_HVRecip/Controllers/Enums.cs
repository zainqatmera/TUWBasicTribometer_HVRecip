using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.Controllers
{
   enum MessageAck
    {
       Accepted = 0,
       Unknown,
       Invalid,
       Busy
    }

    enum TribometerAxis {
        Horizontal,
        Vertical
    }
}
