using System;

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
