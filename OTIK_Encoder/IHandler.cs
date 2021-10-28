using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTIK_Encoder
{
    interface IHandler
    {
        void SetNextHandler();
        bool Handle();
    }
}
