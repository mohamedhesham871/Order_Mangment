using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Exceptions
{
    public  class BadResquestException(string ex):Exception(ex)
    {
        
    }
}
