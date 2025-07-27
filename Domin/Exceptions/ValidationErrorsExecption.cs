using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Exceptions
{
    public class ValidationErrorsExecption(IEnumerable<string> errors):Exception("Invalid Validation Error Occurred")
    {
        public IEnumerable<string> Errors = errors;
    }
}
