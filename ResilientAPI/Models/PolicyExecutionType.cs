using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResilientAPI.Models
{
    [Flags]
    public enum PolicyExecutionType
    {
        Timeout = 1,
        CircuitBreaker = 2,
        Retry = 4
    }
}
