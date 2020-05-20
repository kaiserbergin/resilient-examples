using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResilientAPI.Models
{
    public enum PolicyExecutionType
    {
        Timeout,
        CircuitBreaker,
        Retry
    }
}
