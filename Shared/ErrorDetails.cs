using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ErrorDetails
    {
        public int Status { get; set; }
        public string Message { get; set; }=string.Empty;
        public IEnumerable<string?> Errors { get; set; } = [];
    }
}
